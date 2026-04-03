using System.Diagnostics.Eventing.Reader;
using System.Xml.Linq;

namespace lib
{


	/// <summary>
	/// ツリーノード要素
	/// </summary>
	public class TreeNodeElements
	{
		public string FullPath { get; set; }
		public Color ForeColor { get; set; }
		public int ImageIndex { get; set; }
		public int SelectImageIndex { get; set; }


		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="p_strPath">ツリーノードのフルパス</param>
		/// <param name="p_nImageIndex"></param>
		/// <param name="p_nSelectIndex"></param>
		/// <param name="p_stForeColor"></param>
		public TreeNodeElements( string p_strPath, int p_nImageIndex, int p_nSelectIndex, Color p_stForeColor )
		{
			FullPath = p_strPath;
			ImageIndex = p_nImageIndex;
			SelectImageIndex = p_nSelectIndex;
			ForeColor = p_stForeColor;
		}
	}



	/// <summary>
	/// ツリービューのノードにフルパスでアクセスできるようにする
	/// </summary>
	public class TreeViewEx : TreeView
	{
		#region 外部イベント

		public IProgress<ProgressCtrl> Progress { get; set; } = null;

		#endregion

		#region コンストラクタ

		public TreeViewEx()
		{
		}


		private TreeViewEx( TreeViewEx p_objParent )
		{
			PathSeparator = p_objParent.PathSeparator;
			
		}

		#endregion

		#region メソッド

		/// <summary>
		/// ツリーノードを再帰的に追加します。
		/// </summary>
		/// <param name="p_aryElements">ツリーノード要素の配列</param>
		public async Task<bool> AddNodeRangeAsync( TreeNodeElements[] p_aryElements, bool p_bLazyOpen = false )
		{

			//			Visible = false;
			//			SuspendLayout();

			// ダミーノードにぶら下がってるNodesをルートにする
			TreeNode dummyNode = new TreeNode();


			var lst = await Task.Run(() => process(p_aryElements, dummyNode.Nodes, p_bLazyOpen));

			Nodes.AddRange(lst); // 最後に配置する
								 //			Visible = true;
								 //			ResumeLayout();

			return true;

		}
		
		public void AddNodeRange( TreeNodeElements[] p_aryElements, TreeNodeCollection p_Nodes , bool p_bLazyOpen = false )
		{

			foreach(var element in p_aryElements)
			{
				var leafName = GetLeaf(element.FullPath.Trim(PathSeparator.ToArray()));
				var objNode = ( element.ImageIndex == -1 ) ? p_Nodes.Add(leafName, leafName)
							: p_Nodes.Add(leafName, leafName, element.ImageIndex, element.SelectImageIndex);
				objNode.ForeColor = element.ForeColor;

				if(p_bLazyOpen )
				{
					objNode.Nodes.Add("__dummy__");
					objNode.Tag = false;
				}
			}

		}

		// 非同期処理の主実行
		private TreeNode[] process( TreeNodeElements[] p_aryElements, TreeNodeCollection p_Nodes, bool p_bLazyOpen  )
		{
			ProgressCtrl progress = new ProgressCtrl("展開処理");


			progress.TotalFiles = p_aryElements.Length;
			foreach(var stElement in p_aryElements)
			{
				TreeNode? node = AddNode(stElement, p_Nodes, p_bLazyOpen);

				node.ForeColor = stElement.ForeColor;
				progress.Increment(GetLeaf(stElement.FullPath));
				if ( progress.ProcessedCount % 100 == 0 )
				{
					Progress?.Report(progress);
				}
			}
			return p_Nodes.Cast<TreeNode>().ToArray();



		}


		/// <summary>
		/// 指定パスまでツリーを自動展開する
		/// 例: "D:\Some\Path" → D: → Some → Path を順に展開
		/// </summary>
		public async Task ExpandToPathAsync( string targetPath, Func<TreeNode, Task> p_Func )
		{

			try
			{

				// "D:\Some\Path" → ["D:", "Some", "Path"]
				var segments = SplitPath(targetPath);

				if ( segments.Length == 0 )
				{
					throw new Exception();
				}

				// ルートノード（ドライブ）を探す
				// ルートノードのTextは "D:" などになっている前提
				var nodes = Nodes.Find(segments[0], false);
				if ( nodes.Length == 0 )
				{
					throw new Exception();
				}
				var node = nodes[0];

				// セグメントを順にたどりながら展開
				for ( var idx = 1; idx < segments.Length; idx++ )
				{
					var segment = segments[idx];
					// 未ロードなら子を読み込む
					await p_Func(node);

					// 対象セグメントに一致する子ノードを探す
					nodes = node.Nodes.Find(segment, false);
					if ( nodes.Length == 0 ) break;
					var next = nodes[0];

					if ( next == null ) break; // パスが存在しない

					next.EnsureVisible();
					node = next;
				}

				// 最終ノードを選択・展開
				SelectedNode = node;
				node.Expand();
			}
			catch ( Exception ex )
			{
			}

		}

		/// <summary>
		/// フルパスで指定されたツリービューを追加する
		/// </summary>
		/// <param name="p_strPath">フルパス</param>
		/// <param name="p_nImageIndex">イメージID</param>
		/// <param name="p_nSelectedImageIndex">選択中のイメージID</param>
		/// <returns></returns>
		public TreeNode? AddNode( TreeNodeElements p_stElement,TreeNodeCollection? p_objRoot = null , bool p_bLayzyOpen = false )
		{

			// パスを分割する
			string[] aryTree = SplitPath(p_stElement.FullPath);
			p_objRoot = p_objRoot ?? Nodes;
			var lst = new List<TreeNode>();
			TreeNode? objNode = null; // 追加対象

			// パスを順にたどる
			foreach ( string strNode in aryTree )
			{
				if ( strNode == "" ) continue;

				// すでに存在するか確認
				TreeNode[] objFind = p_objRoot.Find(strNode, false);
				if ( objFind.Length == 0 )
				{
					// なかったら作成する : イメージIDが未設定 / 設定済みでオーバロード切り替え
					objNode = ( p_stElement.ImageIndex == -1 ) ? p_objRoot.Add(strNode, strNode)
							: p_objRoot.Add(strNode, strNode, p_stElement.ImageIndex, p_stElement.SelectImageIndex);

					if ( p_bLayzyOpen )
					{
						// 子ノードのダミーを配置する
						objNode.Nodes.Add("__dummy__");
						objNode.Tag = false;
					}
						// 親ノードを展開する
						objNode.Parent?.ExpandAll();

					p_objRoot = objNode.Nodes;// ノードを子パスに切り替える

				}
				else
				{
					objNode = objFind[0];
					if ( p_bLayzyOpen )
					{
						objNode.Expand();
					}
					p_objRoot = objNode.Nodes; // 見つけたノードの子パスをルートにする
				}

			}

			return objNode;

		}




		// ツリーの葉(ファイルパスでいうファイル名)を取得
		public string GetLeaf( string path)
		{
			int idx = path.LastIndexOf(PathSeparator.ToCharArray()[0]);
			if ( idx < 0 )
				return path;

			return path.Substring(idx + 1);
		}

		// パスを分離する
		public string[] SplitPath(string path)
		{
			char[] sep = PathSeparator.ToCharArray();
			return  path.Trim(sep).Split(sep)
				.Where(s => !string.IsNullOrEmpty(s)).ToArray();
		}

		/// <summary>
		/// フルパスで指定されたツリービューノードを検索する
		/// </summary>
		/// <param name="p_strPath"></param>
		/// <returns></returns>
		public TreeNode? FindNode( string p_strPath )
		{
			// パスを分割する
			string[] aryTree = p_strPath.Split(PathSeparator.ToCharArray());
			TreeNodeCollection objRoot = Nodes;
			TreeNode? objNode = null;
			foreach ( string strNode in aryTree )
			{
				if ( strNode == "" ) continue;
				// すでに存在するか確認
				TreeNode[] objFind = objRoot.Find(strNode, false); // findNode(root, mTreeI);

				if ( objFind.Length > 0 )
				{
					objNode = objFind[0];
					objRoot = objNode.Nodes; // 見つけたノードの子パスをルートにする
				}
				else
				{
					objNode = null;
				}
			}
			return objNode;

		}

		#endregion

	}


}
