using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

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
	class TreeViewEx : TreeView
	{

		#region 外部イベント

		public IProgress<ProgressCtrl> Progress { get; set; }

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
		/// </summary>
		/// <param name="p_aryElements">ツリーノード要素の配列</param>
		public async Task<bool> AddRangeAsync(TreeNodeElements[] p_aryElements)
		{

//			Visible = false;
//			SuspendLayout();

			var lst = await Task.Run(() => process(p_aryElements));

			Nodes.AddRange(lst); // 最後に配置する
//			Visible = true;
//			ResumeLayout();

			return true;

		}

		private TreeNode[] process( TreeNodeElements[] p_aryElements )
		{
			ProgressCtrl pg = new ProgressCtrl(ProcType.Load);

			// ダミーノードにぶら下がってるNodesをルートにする
			TreeNode dummyNode = new TreeNode();

			pg.TotalFiles = p_aryElements.Length;
			for ( int nCnt = 0; nCnt < p_aryElements.Length; nCnt++ )
			{
				TreeNodeElements stElement = p_aryElements[nCnt];
				TreeNode node = AddNode(stElement, dummyNode.Nodes );
				node.ForeColor = stElement.ForeColor;

				pg.Increment();
				Progress?.Report(pg);
			}

			return dummyNode.Nodes.Cast<TreeNode>().ToArray();



		}

		/// <summary>
		/// フルパスで指定されたツリービューを追加する
		/// </summary>
		/// <param name="p_strPath">フルパス</param>
		/// <param name="p_nImageIndex">イメージID</param>
		/// <param name="p_nSelectedImageIndex">選択中のイメージID</param>
		/// <returns></returns>
		public TreeNode AddNode( TreeNodeElements p_stElement, TreeNodeCollection p_objRoot = null)
		{
			// パスを分割する
			string[] aryTree = p_stElement.FullPath.Split(PathSeparator.ToCharArray());
			p_objRoot = p_objRoot ?? Nodes;
			var lst = new List<TreeNode>();
			TreeNode objNode = null; // 追加対象

			// パスを順にたどる
			foreach (string strNode in aryTree)
			{
				if (strNode == "") continue;

				// すでに存在するか確認
				TreeNode[] objFind = p_objRoot.Find(strNode, false);
				if (objFind.Length == 0)
				{
					// なかったら作成する : イメージIDが未設定 / 設定済みでオーバロード切り替え
					objNode = (p_stElement.ImageIndex == -1) ? p_objRoot.Add(strNode, strNode)
							: p_objRoot.Add(strNode, strNode, p_stElement.ImageIndex, p_stElement.SelectImageIndex);
					// 親ノードを展開する
					objNode.Parent?.ExpandAll();
					p_objRoot = objNode.Nodes;// ノードを子パスに切り替える

				}
				else
				{
					objNode = objFind[0];
					p_objRoot = objNode.Nodes; // 見つけたノードの子パスをルートにする
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

	}

}
