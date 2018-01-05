using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;

namespace ControlExtends
{

	public struct TreeNodeElements
	{
		public string strPath;
		public int nImageIndex;
		public int nSelectIndex;
		public Color stForeColor;
		public TreeNodeElements(string p_strPath, int p_nImageIndex, int p_nSelectIndex, Color p_stForeColor)
		{
			strPath = p_strPath;
			nImageIndex = p_nImageIndex;
			nSelectIndex = p_nSelectIndex;
			stForeColor = p_stForeColor;
		}
	}

	/// <summary>
	/// ツリービューのノードにフルパスでアクセスできるようにする
	/// </summary>
	class TreeViewEx : TreeView
	{


		BackgroundWorker worker;
		TreeNodeElements[] m_aryElements;
		TreeViewEx subThreadView;
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
			subThreadView = new TreeViewEx();
			for (int nCnt = 0; nCnt<m_aryElements.Length; nCnt++)
			{
				TreeNodeElements stElement = m_aryElements[nCnt];
				TreeNode node = subThreadView.AddNode(stElement.strPath, stElement.nImageIndex, stElement.nSelectIndex);
				node.ForeColor = stElement.stForeColor;
				worker.ReportProgress(nCnt);
			}
		}

		public event RunWorkerCompletedEventHandler AddNodeRangeComplete;
		public event ProgressChangedEventHandler AddRangeProgress;



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




	}
}
