using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace lib
{

	public class PathTreeManager
	{
		public char Sepalater { get; set;} = '\\';
		private readonly HashSet<string> _paths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		// ★ パス → アイテム一覧
		private readonly Dictionary<string, List<TreeNodeElements>> _itemsByPath = new Dictionary<string, List<TreeNodeElements>>(StringComparer.OrdinalIgnoreCase);

		// ★ キャッシュ（子ノード）
		private readonly Dictionary<string, List<string>> _childrenCache = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

		public TreeNodeElements DefaultPattern { get; set; }

		public int Count => _paths.Count;


		public PathTreeManager()
		{
			DefaultPattern = new TreeNodeElements("",0,1,Color.Black);
		}
		public PathTreeManager(TreeNodeElements defaultPtrn)
		{
			DefaultPattern = defaultPtrn;
		}

		#region 追加

		public void Add( TreeNodeElements item )
		{
			var path = Normalize(item.FullPath);

			_paths.Add(path);
			// パス→アイテム一覧が存在しない？
			if ( !_itemsByPath.TryGetValue(path, out var list) )
			{
				list = new List<TreeNodeElements>();
				_itemsByPath[path] = list;
			}

			list.Add(item);

			_childrenCache.Clear();
		}

		public void AddRange( IEnumerable<TreeNodeElements> items )
		{
			foreach ( var item in items )
			{
				var path = Normalize(item.FullPath);

				_paths.Add(path);

				if ( !_itemsByPath.TryGetValue(path, out var list) )
				{
					list = new List<TreeNodeElements>();
					_itemsByPath[path] = list;
				}

				list.Add(item);
			}

			_childrenCache.Clear();
		}

		#endregion

		#region ツリー取得

		public List<string> GetRoots()
		{
			return _paths
				.Select(p => GetRoot(p))
				.Distinct(StringComparer.OrdinalIgnoreCase)
				.ToList();
		}

		public List<string> GetChildren( string parent )
		{
			parent = Normalize(parent);

			if ( _childrenCache.TryGetValue(parent, out var cached) )
				return cached;

			int parentLen = parent.Length;

			var children = _paths
				.Where(p => p.Length > parentLen &&
							p.StartsWith(parent, StringComparison.OrdinalIgnoreCase))
				.Select(p =>
				{
					var sub = p.Substring(parentLen).TrimStart(Sepalater);
					var idx = sub.IndexOf(Sepalater);
					return idx == -1 ? sub : sub.Substring(0, idx);
				})
				.Where(s => !string.IsNullOrEmpty(s))
				.Distinct(StringComparer.OrdinalIgnoreCase)
				.Select(name => Path.Combine(parent, name))
				.ToList();

			_childrenCache[parent] = children;
			return children;
		}

		#endregion

		#region アイテム取得

		/// <summary>
		/// 完全一致（そのフォルダ直下）
		/// </summary>
		public List<TreeNodeElements> GetItems( string path )
		{
			path = Normalize(path);
			_itemsByPath.TryGetValue(path, out var list);
			if( list == null )
			{
				list = new List<TreeNodeElements>();
				list.Add(DefaultPattern);
			}
			return list;
		}

		

		/// <summary>
		/// 配下すべて（旧IndexOf相当）
		/// </summary>
		public List<TreeNodeElements> GetItemsRecursive( string path )
		{
			path = Normalize(path);

			return _itemsByPath
				.Where(kv => kv.Key.StartsWith(path, StringComparison.OrdinalIgnoreCase))
				.SelectMany(kv => kv.Value)
				.ToList();
		}

		#endregion

		#region ユーティリティ

		private string Normalize( string path )
		{
			return path.TrimEnd(Sepalater);
		}

		private string GetRoot( string path )
		{

			int idx = path.IndexOf(Sepalater);
			if ( idx < 0 )
				return path; // 区切りなし

			return path.Substring(0, idx);

		}

		public string GetLastSegment(string path )
		{
			int idx = Normalize(path).LastIndexOf(Sepalater);
			if ( idx < 0 )
				return path;

			return path.Substring(idx + 1);
		}

		#endregion
	}

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
		public TreeNodeElements(string p_strPath, int p_nImageIndex, int p_nSelectIndex, Color p_stForeColor)
		{
			FullPath = p_strPath;
			ImageIndex = p_nImageIndex;
			SelectImageIndex = p_nSelectIndex;
			ForeColor = p_stForeColor;
		}
	}

	public class TreeViewEx : TreeView
	{
		public PathTreeManager Manager { get; set; }

		private CancellationTokenSource _cts;

		public IProgress<ProgressCtrl> Progress { get; set; } = null;

		public bool PreExpand { get; set; } = true; // 予め展開するか？

		public TreeViewEx()
		{
			BeforeExpand += OnBeforeExpand;
		}

		#region 初期化

		public void Initialize()
		{
			if ( Manager == null ) return;

			BeginUpdate();
			try
			{
				Nodes.Clear();

				foreach ( var root in Manager.GetRoots() )
				{
					Nodes.Add(CreateNode(root));
				}
			}
			finally
			{
				EndUpdate();
			}
		}

		#endregion

		#region ノード生成

		private TreeNode CreateNode( string fullPath )
		{
			var list =  Manager.GetItems(fullPath);
			TreeNode node= new TreeNode(Manager.GetLastSegment(fullPath))
			{
				Tag = fullPath
			};
			
			if(list.Count > 0){
				var man = list[0];
				node.ImageIndex = man.ImageIndex;
				node.SelectedImageIndex = man.SelectImageIndex;
				node.ForeColor = man.ForeColor;
			}

			// 子があるか判定
			if ( Manager.GetChildren(fullPath).Count > 0 )
			{
				node.Nodes.Add("dummy");
			}

			return node;
		}

		#endregion

		#region 展開

		private async void OnBeforeExpand( object sender, TreeViewCancelEventArgs e )
		{
			if ( Manager == null ) return;

			var node = e.Node;
			var path = (string) node.Tag;

			if ( node.Nodes.Count == 1 && node.Nodes[0].Text == "dummy" )
			{
				_cts = new CancellationTokenSource();

				await LoadChildrenAsync(node, path, _cts.Token);
			}
		}

		private async Task LoadChildrenAsync( TreeNode node, string path, CancellationToken ct )
		{
			node.Nodes.Clear();
			node.Nodes.Add("Loading...");

			var children = await Task.Run(() =>
			{
				return Manager.GetChildren(path);
			}, ct);
			ProgressCtrl progressArg = new ProgressCtrl(ProcType.Load);
			progressArg.TotalFiles = children.Count;
			BeginUpdate();
			try
			{
				node.Nodes.Clear();

				int count = 0;

				foreach ( var child in children )
				{
					ct.ThrowIfCancellationRequested();

					node.Nodes.Add(CreateNode(child));
					progressArg.Increment(child);
					Progress?.Report(progressArg);
					if(PreExpand)
						node.ExpandAll();
					if ( ++count % 100 == 0 )
						await Task.Yield();
				}
			}
			finally
			{
				EndUpdate();
			}
		}

		#endregion

		public void Cancel() => _cts?.Cancel();
	}


#if false

	/// <summary>
	/// ツリービューのノードにフルパスでアクセスできるようにする
	/// </summary>
	class TreeViewEx : TreeView
	{

		#region	メンバー変数

		BackgroundWorker worker;
		TreeNodeElements[] m_aryElements;
		TreeViewEx subThreadView;

		#endregion

		#region 外部イベント

		public event RunWorkerCompletedEventHandler AddNodeRangeComplete;
		public event ProgressChangedEventHandler AddRangeProgress;

		#endregion

		#region コンストラクタ

		public TreeViewEx()
		{}

		private TreeViewEx(TreeViewEx p_objParent)
		{
			PathSeparator = p_objParent.PathSeparator;
		}

		#endregion

		#region メソッド

		/// <summary>
		/// ツリーノードを再帰的に追加します。
		/// 　※非同期処理です。
		/// </summary>
		/// <param name="p_aryElements">ツリーノード要素の配列</param>
		public void AddNodeRange(TreeNodeElements[] p_aryElements)
		{
			worker = new BackgroundWorker()
			{
				WorkerReportsProgress = true
			};
			worker.DoWork += Worker_DoWork;
			worker.ProgressChanged += Worker_ProgressChanged;
			worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
			m_aryElements = p_aryElements;
			Visible = false;
			SuspendLayout();
			worker.RunWorkerAsync();
		}

		/// <summary>
		/// フルパスで指定されたツリービューを追加する
		/// </summary>
		/// <param name="p_strPath">フルパス</param>
		/// <param name="p_nImageIndex">イメージID</param>
		/// <param name="p_nSelectedImageIndex">選択中のイメージID</param>
		/// <returns></returns>
		public TreeNode AddNode(string p_strPath, int p_nImageIndex = -1, int p_nSelectedImageIndex = -1)
		{

			// パスを分割する
			string[] aryTree = p_strPath.Split(PathSeparator.ToCharArray());
			TreeNodeCollection objRoot = Nodes; // 追加先ノード
			TreeNode objNode = null; // 追加対象

			// パスを順にたどる
			foreach (string strNode in aryTree)
			{
				if (strNode == "") continue;

				// すでに存在するか確認
				TreeNode[] objFind = objRoot.Find(strNode, false);

				if (objFind.Length == 0)
				{
					// なかったら作成する : イメージIDが未設定 / 設定済みでオーバロード切り替え
					objNode = (p_nImageIndex == -1) ? objRoot.Add(strNode, strNode)
							: objRoot.Add(strNode, strNode, p_nImageIndex, p_nSelectedImageIndex);
					// 親ノードを展開する
					if (objNode.Parent != null)
					{
						objNode.Parent.ExpandAll();
					}
					objRoot = objNode.Nodes;// ノードを子パスに切り替える

				}
				else
				{
					objNode = objFind[0];
					objRoot = objNode.Nodes; // 見つけたノードの子パスをルートにする

				}

			}
			return objNode;

		}

		/// <summary>
		/// フルパスで指定されたツリービューノードを検索する
		/// </summary>
		/// <param name="p_strPath"></param>
		/// <returns></returns>
		public TreeNode FindNode(string p_strPath)
		{
			// パスを分割する
			string[] aryTree = p_strPath.Split(PathSeparator.ToCharArray());
			TreeNodeCollection objRoot = Nodes;
			TreeNode objNode = null;
			foreach (string strNode in aryTree)
			{
				if (strNode == "") continue;
				// すでに存在するか確認
				TreeNode[] objFind = objRoot.Find(strNode, false); // findNode(root, mTreeI);

				if (objFind.Length > 0)
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

		#region	非同期イベント

		private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			TreeNode[] ctl = new TreeNode[subThreadView.Nodes.Count];
			subThreadView.Nodes.CopyTo(ctl, 0);
			subThreadView.Nodes.Clear();

			Nodes.AddRange(ctl);
			Visible = true;
			ResumeLayout();
			AddNodeRangeComplete?.Invoke(this, e);
		}

		private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			AddRangeProgress?.Invoke(this, e);
		}

		private void Worker_DoWork(object sender, DoWorkEventArgs e)
		{
			subThreadView = new TreeViewEx(this);
			for (int nCnt = 0; nCnt<m_aryElements.Length; nCnt++)
			{
				TreeNodeElements stElement = m_aryElements[nCnt];
				TreeNode node = subThreadView.AddNode(stElement.strPath, stElement.nImageIndex, stElement.nSelectIndex);
				node.ForeColor = stElement.stForeColor;
				worker.ReportProgress(nCnt);
			}
		}

		#endregion

	}

#endif
}
