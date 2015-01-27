using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KanColleTable
{


	public partial class Form1 : Form
	{
		private Dictionary<String, float> devData_Rare;//右側、統計データ表示用リスト(レア度および失敗)
		private Dictionary<String, float> devData_Category;//右側、統計データ表示用リスト(カテゴリ)
		private Dictionary<String, float> devData_Equipment;//右側、統計データ表示用リスト(装備名)

		private List<String> Rare_keyList, Category_keyList, Equipment_keyList;
		private List<String> set;

		private List<String> lineList = new List<String>();//読み込んだ行情報を格納
		private String[,] data;//多次元配列の定義は[][]ではなく[,]


		//ウィンドウをクライアント領域内でドラッグするためのもの
		private const int WM_NCHITTEST = 0x84;
		private const int HTCLIENT = 0x1;
		private const int HTCAPTION = 0x2;

		//タイトルバーダブルクリック時の最大化を抑止
		private const int WM_SYSCOMMAND = 0x112;
		private const int SC_MAXIMIZE = 0xF030;

		//装備名
		Object[] choice0 = { "成功", "失敗" };

		Object[] choice1 = {
			"装備の種類", //0
			"主砲", //1
			"主砲(対空砲)", //2
			"副砲", //3
			"魚雷", //4
			"艦載機", //5
			"電探", //6
			"機関", //7
			"砲弾", //8
			"ソナー", //9
			"爆雷", //10
			"対空機銃", //11
			"その他", //12
	    };

		Object[] syuhou = {
			"装備名を選択",
			//主砲
			//小口径
			"12cm単装砲",
			"12.7cm連装砲",
			//"10cm連装高角砲",
			"12.7cm連装砲B型改二",
			//中口径
			"14cm単装砲",
			"15.5cm三連装砲(主砲)",
			"20.3cm連装砲",
			"20.3cm(3号)連装砲",
			"15.2cm連装砲",
			//大口径
			"35.6cm連装砲",
			"41cm連装砲",
			"46cm三連装砲",
	    };

		Object[] taikuu = {
			"装備名を選択",
			//主砲（対空砲）
			"10cm連装高角砲",
			"12.7cm単装高角砲",
			"12.7cm連装高角砲",//?
			"8cm高角砲",//?
			"10cm連装高角砲(砲架)",//?
	    };
		Object[] hukuhou = {
			"装備名を選択",
			//副砲
			"12.7cm連装高角砲",//?
			"15.2cm単装砲",
			"15.5cm三連装砲(副砲)",
			"8cm高角砲",//?
			"10cm連装高角砲(砲架)",//?
	    };
		Object[] gyorai = {
			"装備名を選択",
			//魚雷
			"61cm三連装魚雷",
			"61cm四連装魚雷",
			"61㎝四連装(酸素)魚雷",
			"61㎝五連装(酸素)魚雷",
			"53㎝艦首(酸素)魚雷",

			//特殊潜航艇
			"甲標的 甲",
	    };
		Object[] kansaiki = {
			"装備名を選択",
			//艦載機
			//艦上戦闘機
			"九六式艦戦",
			"零式艦戦21型",
			"零式艦戦52型",
			"烈風",
			"紫電改二",
			"震電改",
			//"烈風改",

			//艦上攻撃機
			"九七式艦攻",
			"天山",
			"流星",
			"流星改",

			//艦上爆撃機
			"九九式艦爆",
			"彗星",
			"彗星一二型甲",
			"零式艦戦62型(爆戦)",
			"Ju87C改",

			//艦上偵察機
			"彩雲",
			"二式艦上偵察機",

			//水上機
			"零式水上偵察機",
			"瑞雲",
			"零式水上観測機",
			"試製晴嵐",

			//上陸用舟艇
			"大発動艇",
	    };
		Object[] dentan = {
			"装備名を選択",
			//電探

			//小型電探
			"13号対空電探",
			"22号対水上電探",
			"33号対水上電探",

			//大型電探
			"21号対空電探",
			"32号対水上電探",
			"14号対空電探",
	    };
		Object[] kikan = {
		    	"装備名を選択",
			//機関
			"改良型艦本式タービン",
			"強化型艦本式缶",
	    };
		Object[] houdan = {
			"装備名を選択",
			//砲弾
			"三式弾",
			"九一式徹甲弾",
	    };
		Object[] sonar = {
			"装備名を選択",
			//ソナー
			"九三式水中聴音機",
			"三式水中探信儀",
	    };
		Object[] bakurai = {
			"装備名を選択",
			//ソナー
			"九四式爆雷投射機",
			"三式爆雷投射機",
	    };
		Object[] kijuu = {
			"装備名を選択",
			//対空機銃
			"7.7mm機銃",
			"12.7mm単装機銃",
			"25mm連装機銃",
			"25mm三連装機銃",
			"25mm単装機銃",
			"12cm30連装噴進砲",
	    };
		Object[] etc = {
			"装備名を選択",
			//その他
			"応急修理要員",
			"応急修理女神",
	    };

		Object[] choice3 = {
			"レア度",
			"1", "2", "3", "4", "5",
	    };

		public Form1()
		{
			InitializeComponent();//デザイナーによる自動生成
			MyInitialize();//デザイナーでできない初期化
			
			//this.comboBox1.SelectedIndex = 0;
			this.comboBox2.SelectedIndex = 0;
			this.comboBox3.SelectedIndex = 0;
			this.comboBox4.SelectedIndex = 0;

			//comboBoxの初期化後にcomboBox1になんかフォーカスするの嫌だけどどうすればいいかわからんのでとりま別のとこににフォーカス
			this.ActiveControl = this.dataGridView1;

		}

		private void MyInitialize()
		{
			for (int LvNum = 100; LvNum < 151; LvNum++)
			{
				this.comboBox5.Items.AddRange(new object[] { LvNum });
			}

			this.dataGridView1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			// 
			// comboBox1ｃ
			// 
			//this.comboBox1.Items.AddRange(choice0);
			//this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
			

			// 
			// comboBox2
			// 
			this.comboBox2.Items.AddRange(choice1);
			this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);

			// 
			// comboBox3
			// 
			this.comboBox3.Items.AddRange(new object[] { "-" });
			this.comboBox3.SelectedIndexChanged += new System.EventHandler(this.comboBox3_SelectedIndexChanged);
			
			
			// 
			// comboBox4
			// 
			this.comboBox4.Items.AddRange(new object[] { "-" });
			this.comboBox4.SelectedIndexChanged += new System.EventHandler(this.comboBox4_SelectedIndexChanged);
			

			//統計データ初期化
			
			devData_Rare      = new Dictionary<string,float>();//右側、統計データ表示用リスト(レア度および失敗)
			devData_Category  = new Dictionary<string,float>();//右側、統計データ表示用リスト(カテゴリ)
			devData_Equipment = new Dictionary<string,float>();//右側、統計データ表示用リスト(装備名)

			Rare_keyList      = new List<string>(); //統計データレア度リスト
			Category_keyList  = new List<string>(); //統計データカテゴリリスト
			Equipment_keyList = new List<String>(); //統計データ装備名リスト
			set = new List<string>();

			this.calcData(); //統計データの計算、格納

			this.drawData(); //統計データの表示
		}


		private void Form1_Load(object sender, EventArgs e)
		{
			
		}

		private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			calcData();
			drawData();
		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{

		}

		private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{

		}

		private void FileToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
		{
			//インスタンス生成
			OpenFileDialog ofd = new OpenFileDialog();

			//デフォルトのファイル名設定
			ofd.FileName = "";
			//デフォルトのフォルダ名設定
			Assembly myAssemby = Assembly.GetEntryAssembly();
			string myPath = myAssemby.Location;

			ofd.InitialDirectory = @myPath;
			//ファイルの種類に表示される選択肢設定
			ofd.Filter = "テキストファイル(*.txt)|*.txt;|全てのファイル(*.*)|*.*";
			//ファイルの種類ではじめに選択されている選択肢の設定(1～)
			ofd.FilterIndex = 1;
			//タイトル設定
			ofd.Title = "開くファイルを選択して下さい";
			//ダイアログボックスを閉じる前に現在のディレクトリを復元できるようにする
			ofd.RestoreDirectory = true;
			//存在しないファイル名を指定した時の警告
			ofd.CheckFileExists = true;
			//存在しないパスが指定された時の警告(デフォルトでTrueなので必要ない)
			ofd.CheckPathExists = true;

			//ダイアログの表示
			if (ofd.ShowDialog() == DialogResult.OK)
			{
				//ファイル選択時の挙動
				//Console.WriteLine(ofd.FileName);
				//Console.WriteLine(FileRead(ofd.FileName));
				FileRead(ofd.FileName);

			}
		}


		private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			//インスタンス生成
			SaveFileDialog sfd = new SaveFileDialog();

			sfd.FileName = "";

			Assembly myAssemby = Assembly.GetEntryAssembly();
			string myPath = myAssemby.Location;

			//sfd.InitialDirectory = @"C:\";
			sfd.InitialDirectory = @myPath;
			
			sfd.Filter = "テキストファイル(*.txt)|*.txt|すべてのファイル(*.*)|*.*";
			sfd.FilterIndex = 1;
			//タイトル設定
			sfd.Title = "名前をつけて保存";
			sfd.RestoreDirectory = true;
			//既に存在するファイル名を指定した時に警告する
			sfd.OverwritePrompt = true;
			sfd.CheckPathExists = true;

			//ダイアログの表示
			if (sfd.ShowDialog() == DialogResult.OK)
			{
				//OKボタンがクリックされた時の挙動
				//Console.WriteLine(sfd.FileName);
				FileWrite(sfd.FileName);
			}

		}

		private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		//private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		//{
		//	//Console.WriteLine("ここ");
		//	comboBox2.Items.Clear();

		//	switch (comboBox1.SelectedIndex)
		//	{
		//		case 0:
		//			comboBox2.Items.AddRange(choice1);
		//			break;

		//		case 1:
		//			comboBox2.Items.AddRange(new object[] { "-" });
		//			break;

		//		default:
		//			Console.WriteLine("えらー");
		//			break;

		//	}

		//	comboBox2.SelectedIndex = 0;
		//	comboBox3.SelectedIndex = 0;
		//	comboBox4.SelectedIndex = 0;

		//}

		private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
		{
			//Console.WriteLine("ここ");
			comboBox3.Items.Clear();

			switch (comboBox2.SelectedIndex)
			{
				case 0:
					comboBox3.Items.AddRange(new object[] { "-" });
					comboBox4.Items.Clear();
					comboBox4.Items.AddRange(new object[] { "-" });
					break;
				case 1:
					comboBox3.Items.AddRange(syuhou);
					break;
				case 2:
					comboBox3.Items.AddRange(taikuu);
					break;
				case 3:
					comboBox3.Items.AddRange(hukuhou);
					break;
				case 4:
					comboBox3.Items.AddRange(gyorai);
					break;
				case 5:
					comboBox3.Items.AddRange(kansaiki);
					break;
				case 6:
					comboBox3.Items.AddRange(dentan);
					break;
				case 7:
					comboBox3.Items.AddRange(kikan);
					break;
				case 8:
					comboBox3.Items.AddRange(houdan);
					break;
				case 9:
					comboBox3.Items.AddRange(sonar);
					break;
				case 10:
					comboBox3.Items.AddRange(bakurai);
					break;
				case 11:
					comboBox3.Items.AddRange(kijuu);
					break;
				case 12:
					comboBox3.Items.AddRange(etc);
					break;
				default:
					Console.WriteLine("えらー");
					break;
			}
			comboBox3.SelectedIndex = 0;
			comboBox4.SelectedIndex = 0;
		}

		private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
		{
			//Console.WriteLine("ここ");
			if (!comboBox3.Items.Contains("-"))
			{
				comboBox4.Items.Clear();
				switch (0)
				{
					case 0:
						comboBox4.Items.AddRange(choice3);
						break;

					//default:
					//    Console.WriteLine("えらー");
					//    break;

				}
				comboBox4.SelectedIndex = 0;
			}
		}

		private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		private void button1_Click(object sender, EventArgs e)
		{
			String[] dataA = { (dataGridView1.Rows.Count + 1).ToString(), "成功", comboBox2.Text, comboBox3.Text, comboBox4.Text };
			dataGridView1.Rows.Add(dataA);
			calcData();
			drawData();

			dataGridView1.ClearSelection();
			dataGridView1.Rows[dataGridView1.Rows.Count - 1].Selected = true;
			dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.Rows.Count - 1;

		}

		private void label2_Click(object sender, EventArgs e)
		{

		}

		private void label5_Click(object sender, EventArgs e)
		{

		}


		/*-------------------自前--------------------*/
		private void FileRead(String filePath)
		{
			//結果格納変数
			//StringBuilder strBuff = new StringBuilder();
			String strBuff;
			String result = string.Empty;
			//ファイルの存在チェック
			if (System.IO.File.Exists(filePath))
			{
				//StreamReaderでファイルを読み込む
				System.IO.StreamReader reader = (new System.IO.StreamReader(filePath, System.Text.Encoding.GetEncoding("shift_jis")));

				//最初の3行(レシピ、旗艦、旗艦Lv)を読み込む
				String[] firstLineData = new String[3];

				for (int i = 0; i < 3; i++)
				{
					if ((strBuff = reader.ReadLine()) != null)
					{
						//Console.WriteLine(strBuff);
						firstLineData[i] = strBuff;
					}
				}

				while (reader.Peek() >= 0)
				{
					//ファイルを1行ずつ読み込む
					strBuff = reader.ReadLine();
					////読み込んだものを追加で格納する
					//result += strBuff + System.Environment.NewLine;
					//Console.WriteLine(strBuff);
					lineList.Add(strBuff);
				}
				reader.Close(); //リソースの破棄

				textBox2.Text = firstLineData[0];
				textBox3.Text = firstLineData[1];
				comboBox5.Text = firstLineData[2];

				data = new String[lineList.Count, dataGridView1.ColumnCount];//多次元配列の宣言方法に注意
				String[] splitLine = new String[dataGridView1.ColumnCount];
				//foreach(String line in lineList)
				//{
				//}
				for (int i = 0; i < lineList.Count; i++)
				{
					splitLine = lineList[i].Split(','); //区切り文字の指定は""ではなく''

					for (int j = 0; j < dataGridView1.ColumnCount; j++)
					{
						if (splitLine[j] != null)
							data[i, j] = splitLine[j];
						else
							data[i, j] = "null";
					}

				}

				dataGridView1.Rows.Clear();
				for (int x = 0; x < lineList.Count; x++)
				{
					this.dataGridView1.Rows.Add(data[x, 0], data[x, 1], data[x, 2], data[x, 3], data[x, 4]);
				}

			}
			calcData();
			drawData();

			dataGridView1.ClearSelection();
			dataGridView1.Rows[dataGridView1.Rows.Count - 1].Selected = true;
			dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.Rows.Count - 1;
		}

		private void FileWrite(String filePath)
		{
			System.IO.StreamWriter writer = new System.IO.StreamWriter(filePath, false, System.Text.Encoding.GetEncoding("shift_jis"));

			//最初の3行(レシピ、旗艦、旗艦Lv)を書き出す
			String[] firstLineData = new String[3];

			firstLineData[0] = textBox2.Text;
			firstLineData[1] = textBox3.Text;
			firstLineData[2] = comboBox5.Text;

			for (int i = 0; i < 3; i++)
			{
				writer.Write(firstLineData[i] + "\n");
			}
			//データ書き出し
			String writeingLine;
			for (int y = 0; y < dataGridView1.Rows.Count; y++)
			{
				writeingLine = String.Empty;

				for (int x = 0; x < dataGridView1.ColumnCount; x++)
				{
					writeingLine += dataGridView1[x, y].Value;

					if (x != dataGridView1.ColumnCount - 1) writeingLine += ",";
				}
				writer.Write(writeingLine + "\n");
			}

			writer.Close();
		}

		private void calcData()
		{
			Rare_keyList.Clear();
			devData_Rare.Clear();
			Category_keyList.Clear();
			devData_Category.Clear();
			Equipment_keyList.Clear();
			devData_Equipment.Clear();

			//レア度分布計算
			for (int i = 0; i < dataGridView1.Rows.Count; i++)
			{
				if (!Rare_keyList.Contains(dataGridView1[4, i].Value))
				{
					Rare_keyList.Add((string)dataGridView1[4, i].Value);
					devData_Rare.Add((string)dataGridView1[4, i].Value, 1.0f);
				}
				else
				{
					devData_Rare[(string)dataGridView1[4, i].Value] += 1.0f;
				}
			}

			foreach (String str in Rare_keyList)
			{
				if (Rare_keyList.Contains(str))
				{
					devData_Rare[str] = 100 * devData_Rare[str] / dataGridView1.Rows.Count;
				}
			}

			//カテゴリ分布計算
			for (int i = 0; i < dataGridView1.Rows.Count; i++)
			{
				if (!Category_keyList.Contains(dataGridView1[2, i].Value))
				{
					Category_keyList.Add((string)dataGridView1[2, i].Value);
					devData_Category.Add((string)dataGridView1[2, i].Value, 1.0f);
				}
				else
				{
					devData_Category[(string)dataGridView1[2, i].Value] += 1.0f;
				}
			}

			foreach (String str in Category_keyList)
			{
				if (Category_keyList.Contains(str))
				{
					devData_Category[str] = 100 * devData_Category[str] / dataGridView1.Rows.Count;
				}
			}

			//装備名分布カウント
			for (int i = 0; i < dataGridView1.Rows.Count; i++)
			{
				if (!Equipment_keyList.Contains(dataGridView1[3, i].Value))
				{
					Equipment_keyList.Add((string)dataGridView1[3, i].Value);
					devData_Equipment.Add((string)dataGridView1[3, i].Value, 1.0f);
				}
				else
				{
					devData_Equipment[(string)dataGridView1[3, i].Value] += 1.0f;
				}
			}

		}

		private void drawData()
		{		
			textBox1.Text = "";//クリア

			textBox1.AppendText("------レア度------\n");
			foreach(String str in devData_Rare.Keys)
			{
				if (str == "-")
				{
					textBox1.AppendText("失敗" + "  " + devData_Rare[str] + "%\n");
				}
				else
				{
					textBox1.AppendText(str + "  " + devData_Rare[str] + "%\n");
				}
			}

			textBox1.AppendText("\n------種   類------\n");
			foreach (String str in devData_Category.Keys)
			{
				if (str != "-")
				{
					textBox1.AppendText(str + "  " + devData_Category[str] + "%\n");
				}
			}

			textBox1.AppendText("\n------装備名------\n");
			foreach (String str in devData_Equipment.Keys)
			{
				if (str != "-")
				{
					textBox1.AppendText(str + "  " + devData_Equipment[str] + "個  " + devData_Equipment[str] * 100 / dataGridView1.Rows.Count +"%\n");
				}
			}

		}

		/*-------------オーバーライド-----------------*/
		protected override void WndProc(ref Message m)   //メッセージ処理の改変
		{
			switch (m.Msg)
			{
				case WM_NCHITTEST:　//タイトルバーにマウスがあるふりをさせるa
					base.WndProc(ref m);
					if ((int)m.Result == HTCLIENT)
					{
						m.Result = (IntPtr)HTCAPTION;
						return;
					}
					break;

				case WM_SYSCOMMAND: //タイトルバーダブルクリック時の最大化を抑止
					if ((m.WParam.ToInt32() & SC_MAXIMIZE) == SC_MAXIMIZE)
					{
						return;
					}

					break;

			}
			base.WndProc(ref m);
		}

		private void button2_Click(object sender, EventArgs e)
		{


			this.Close();
		}

		private void button3_Click(object sender, EventArgs e)
		{
			this.WindowState = FormWindowState.Minimized;
		}

		private void button4_Click(object sender, EventArgs e)
		{
			String[] dataA = { (dataGridView1.Rows.Count + 1).ToString(), "失敗", "-", "-", "-" };
			dataGridView1.Rows.Add(dataA);
			calcData();
			drawData();

			dataGridView1.ClearSelection();
			dataGridView1.Rows[dataGridView1.Rows.Count - 1].Selected = true;
			dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.Rows.Count - 1;
		}

		private void textBox2_TextChanged(object sender, EventArgs e)
		{

		}

		private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

	}
}
