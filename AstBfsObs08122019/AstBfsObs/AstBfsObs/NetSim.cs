//******************************************************************************************************//
//**********************************Mrudang T. Mehta, Himanshu S. Mazumdar ************************//
//******************************************************************************************************//using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace AstBfsObs
{
    public partial class NetSim : Form
    {
        //**********************************************************
        PointF[] nods;
        float[] nodSz;
        ArrayList nod2dNew;
        ArrayList closeNods;
        Random rnd = new Random(DateTime.Now.Millisecond);
        Bitmap bm;
        float d30 = 20;
        int[] grpSz;
        Brush brs0 = Brushes.Green;
        int srcNo = -1;
        int dstNo = -1;
        int curNo = -1;
        float grpW;
        float grpH;
        int totalMsg = 0;
        ArrayList polyList;
        ArrayList resultList2 = new ArrayList();
        int astr = 0;
        int bfs = 0;
        int loop = 0;
        ArrayList resultList3 = new ArrayList();
        int[] sd;
        bool obstFlg = false;
        int countMx;
        int count2;
        int count3;
        StreamWriter sw0;
        ArrayList anlysis = new ArrayList();
        //**********************************************************
        public NetSim()
        {
            InitializeComponent();
        }
        //**********************************************************
        private void NetSim_Shown(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            ClearScreen();
            CreateAdvancedNodes();
            ///PopulateNode2D();//think?
            DrawNodes(true);
            btnReDraw_Click(null, null);
        }
        //**********************************************************
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //**********************************************************
        private void ClearScreen()
        {
            bm = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(bm);
            g.Clear(Color.Blue);
            g.Dispose();
            pictureBox1.Image = bm;
            nods = new PointF[0];
        }
        //***********************************************************
        private void CreateAdvancedNodes()
        {
            obstFlg = false;
            bm = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            int mxNods = int.Parse(textBox1.Text);
            nods = new PointF[mxNods];
            nodSz = new float[mxNods];
            for (int i = 0; i < nodSz.Length; i++)
            {
                nodSz[i] = 1.0f;
            }
            int zoneX = int.Parse(txbxZones.Text.Split(',')[0]);
            int zoneY = int.Parse(txbxZones.Text.Split(',')[1]);
            float nodsGrp = (float)mxNods / (zoneX * zoneY);
            grpW = (float)pictureBox1.Width / zoneX;
            grpH = (float)pictureBox1.Height / zoneY;
            grpSz = new int[zoneX * zoneY];
            PointF[] grpOrg = new PointF[zoneX * zoneY];
            int n = 0;
            float sum = 0;
            int s = 0;
            float h = 0;
            int sno = 0;
            int n2 = 0;
            for (int i = 0; i < grpSz.Length; i++)
            {
                int n1 = mxNods / grpSz.Length;
                n2 += mxNods - (grpSz.Length * n1);

                if (n2 >= grpSz.Length)
                {
                    n2 -= grpSz.Length;
                    n1++;

                }
                s += n1;
                grpSz[i] = n1;

            }
            for (int y = 0; y < zoneY; y++)
            {
                float w = 0;
                for (int x = 0; x < zoneX; x++)
                {
                    sum += nodsGrp;
                    n++;

                    sum -= (int)sum % n;
                    grpOrg[n - 1] = new PointF(w, h);
                    w += grpW;
                    for (int i = 0; i < grpSz[n - 1]; i++)
                    {
                        float xp = (float)(grpOrg[n - 1].X + rnd.NextDouble() * grpW);
                        float yp = (float)(grpOrg[n - 1].Y + rnd.NextDouble() * grpH);
                        nods[sno++] = new PointF(xp, yp);
                    }
                }
                h += grpH;
            }
            //label5.Text = s.ToString();
           /// textBox1.Text = s.ToString();
            //PopulateNode2D();
            //DrawNodes(true);
        }
        //***********************************************************
        private void PopulateNode2D()
        {
            DrawObstacle();
            nod2dNew = new ArrayList();
            timer3.Interval = 1;
            timer3.Start();
        }
        //***********************************************************
        private void DrawNodes(bool clr)
        {
            int zoneX = int.Parse(txbxZones.Text.Split(',')[0]);
            int zoneY = int.Parse(txbxZones.Text.Split(',')[1]);
            float grpW = (float)pictureBox1.Width / zoneX;
            float grpH = (float)pictureBox1.Height / zoneY;
            Graphics g = Graphics.FromImage(bm);
            if (clr)
                g.Clear(Color.Blue);
            if (chkbxGrid.Checked)
            {
                for (int yn = 0; yn <= zoneY; yn++) //Row
                {
                    float y = yn * grpH;
                    g.DrawLine(Pens.LightBlue, new PointF(0, y), new PointF(bm.Width - 1, y));
                }
                for (int xn = 0; xn <= zoneX; xn++) //Column
                {
                    float x = xn * grpW;
                    g.DrawLine(Pens.LightBlue, new PointF(x, 0), new PointF(x, bm.Height - 1));
                }
            }
            for (int n = 0; n < nods.Length; n++)
            {
                float nd = 1 + d30 * nodSz[n];
                float nd2 = nd / 2;
                g.FillEllipse(Brushes.Yellow, nods[n].X - nd2, nods[n].Y - nd2, nd, nd);

            }
            g.Dispose();
            pictureBox1.Image = bm;
        }
        //***********************************************************
        private string[] ReturnNeibours(int n)
        {
            int txDstFull = (int)Math.Sqrt(bm.Width * bm.Width + bm.Height * bm.Height);
            int txDstMx = (int)Math.Sqrt(grpW * grpW + grpH * grpH);
            ArrayList al0 = new ArrayList();
            int iMx = 0;
            for (int i = 0; i < nods.Length; i++)
            {
                if (i != n)
                {
                    int dist = (int)Math.Sqrt((nods[n].X - nods[i].X) * (nods[n].X - nods[i].X) + (nods[n].Y - nods[i].Y) * (nods[n].Y - nods[i].Y));
                    if (chkbxObst.Checked)
                    {
                        obstFlg = true;
                        Point[] pts = GetLine((int)nods[n].X, (int)nods[n].Y, (int)nods[i].X, (int)nods[i].Y);
                        for (int j = 0; j < pts.Length; j++)
                        {
                            Color col = bm.GetPixel(pts[j].X, pts[j].Y);
                            if ((col.R == Color.Red.R) & (col.G == Color.Red.G) & (col.B == Color.Red.B))
                            {
                                dist += bm.Width;
                                break;
                            }
                        }
                    }
                    if (dist < txDstMx)
                        iMx++;
                    al0.Add(dist.ToString().PadLeft(5, '0') + "," + i.ToString());
                }
            }
            al0.Sort();
            string[] st = new string[3];
            st[0] = "";
            st[1] = "";
            st[2] = iMx.ToString();
            for (int i = 0; i < al0.Count; i++)
            {
                string s2 = al0[i].ToString();
                st[0] += s2.Split(',')[1] + ",";
                st[1] += s2.Split(',')[0] + ",";
            }
            st[0] = st[0].TrimEnd(',');
            st[1] = st[1].TrimEnd(',');
            return st;
        }
        //**********************************************************
        Point[] GetLine(int x1, int y1, int x2, int y2)
        {
            int sx = 1;
            if (x2 < x1)
                sx = -1;
            int sy = 1;
            if (y2 < y1)
                sy = -1;
            int xSiz = Math.Abs(x1 - x2);
            int ySiz = Math.Abs(y1 - y2);
            int xBuc = 0;
            int yBuc = 0;
            int x = 0;
            int y = 0;
            ArrayList al = new ArrayList();
            if (xSiz < ySiz)
            {
                for (; ; )
                {
                    al.Add(x.ToString() + "," + y.ToString());
                    yBuc += xSiz;
                    y++;
                    if (y >= ySiz)
                        break;
                    if (yBuc >= ySiz)
                    {
                        x++;
                        if (x >= xSiz)
                            break;
                        yBuc -= ySiz;//
                    }
                }
                al.Add(x.ToString() + "," + y.ToString());
            }
            else
            {
                for (; ; )
                {
                    al.Add(x.ToString() + "," + y.ToString());
                    xBuc += ySiz;
                    x++;
                    if (x >= xSiz)
                        break;
                    if (xBuc >= xSiz)
                    {
                        y++;
                        if (y >= ySiz)
                            break;
                        xBuc -= xSiz;//
                    }
                }
                al.Add(x.ToString() + "," + y.ToString());
            }
            Point[] ptLn = new Point[al.Count];
            for (int i = 0; i < al.Count; i++)
            {
                string[] wrds = al[i].ToString().Split(',');
                int xx1 = int.Parse(wrds[0]);
                int yy1 = int.Parse(wrds[1]);
                int xx = (int)(x1 + sx * xx1);
                int yy = (int)(y1 + sy * yy1);
                ptLn[i] = new Point(xx, yy);
            }
            return ptLn;
        }
        //**********************************************************
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearScreen();
            lblSrcDst.Text = "";
            srcNo = -1;
            dstNo = -1;
            chkbxGrid.Checked = false;
        }
        //**********************************************************
        private void btnNodes_Click(object sender, EventArgs e)
        {
            ClearScreen();
            pictureBox1.Refresh();
            CreateAdvancedNodes();
            PopulateNode2D();
            DrawNodes(true);
            resultList2 = new ArrayList();
            if (chkbxObst.Checked)
            {
                chkbxObst_CheckedChanged(null, null);
            }
            srcNo = -1;
            dstNo = -1;
            lblSrcDst.Text = "";
        }
        //**********************************************************
        private void btnSrcDst_Click(object sender, EventArgs e)
        {
            GetSrcDst(false);
            if (srcNo >= 0)
                lblSrcDst.Text = srcNo.ToString() + "," + dstNo.ToString();
            lblBFCnt.Text = "";
            lblAstarCnt.Text = "";
        }
        //**********************************************************
        private void GetSrcDst(bool noRnd)
        {
            if (nods.Length == 0)
                return;
            bool flg = false;
            Brush srcBrs = Brushes.DarkGreen;
            Brush dstBrs = Brushes.DarkRed;
            Pen linePen = Pens.Khaki;
            if (noRnd == false)
            {
                srcNo = rnd.Next(nods.Length);
                dstNo = -1;
                for (; ; )
                {
                    dstNo = rnd.Next(nods.Length);
                    if ((srcNo != dstNo))
                    {
                        if (nods[srcNo].X > bm.Width / 2 & nods[dstNo].X < bm.Width / 2)
                            break;
                        if (nods[srcNo].X < bm.Width / 2 & nods[dstNo].X > bm.Width / 2)
                            break;
                    }
                }
                flg = true;
            }
            PlotSrcDst(bm, srcNo, srcBrs, dstNo, dstBrs, linePen, flg);//true
        }
        //***********************************************************
        private void PlotSrcDst(Bitmap bm, int srcNo, Brush srcBrs, int dstNo, Brush dstBrs, Pen linePen, bool flg)
        {
            if (srcNo < 0)
                return;
            if (nods.Length <= 0)
                return;
            if (flg)
                DrawNodes(true);
            float ndS = 1 + d30 * nodSz[srcNo];
            float ndS2 = ndS / 2;
             float ndD = 1 + d30 * nodSz[dstNo];
            float ndD2 = ndD / 2;
            Graphics g = Graphics.FromImage(bm);
            if (chkbxObst.Checked)                         // Mendatory for Astar with Obstacle Nodes
            {
                for (int i = 0; i < polyList.Count; i++)
                {
                    PointF[] poly2 = (PointF[])polyList[i];
                    g.FillPolygon(Brushes.Red, poly2);
                }
            }
            g.FillEllipse(srcBrs, nods[srcNo].X - ndS2, nods[srcNo].Y - ndS2, ndS, ndS);
            g.FillEllipse(dstBrs, nods[dstNo].X - ndD2, nods[dstNo].Y - ndD2, ndD, ndD);
            g.DrawLine(linePen, new PointF(nods[srcNo].X, nods[srcNo].Y), new PointF(nods[dstNo].X, nods[dstNo].Y));
            g.Dispose();
            pictureBox1.Image = bm;
        }
        //**********************************************************
        private void btnAStar_Click(object sender, EventArgs e)
        {
            if (srcNo == -1)
                return;
            if (dstNo == -1)
                return;
            DoAStarBsf(false);
            resultList2.Add(closeNods);
            GetSrcDst(true);
            lblAstarCnt.Text = astr.ToString();
        }
        //**********************************************************
        private void btnBstFst_Click(object sender, EventArgs e)
        {
            if (srcNo == -1)
                return;
            if (dstNo == -1)
                return;
            DoAStarBsf(true);
            resultList2.Add(closeNods);
            GetSrcDst(true);
            lblBFCnt.Text = bfs.ToString();
        }
        //***********************************************************
        private void DoAStarBsf(bool bstFst)
        {
            int[] node2dst = GetNodeSrtdDistList(nod2dNew, dstNo);//include self(dstNo)
            curNo = srcNo;
            closeNods = new ArrayList();
            closeNods.Add(curNo);
            for (; ; )
            {
                string[] st20 = (string[])nod2dNew[curNo];
                int[] srcLst20 = Str2IntArr(st20[0].ToString().Split(','));
                int[] srcLst21 = Str2IntArr(st20[1].ToString().Split(','));
                int nbrMx = int.Parse(st20[2]);
                int iMn = -1;
                int iMn2 = -1;
                float fCostMin = float.MaxValue;
                for (int i2 = 0; i2 < nbrMx; i2++)
                {
                    int ng1 = srcLst20[i2];
                    int ng = GetNg(ng1, closeNods);
                    if (ng >= 0)
                    {
                        int nowNo = (int)closeNods[closeNods.Count - 1];
                        float hCost = node2dst[ng];
                        float gCost = srcLst21[i2] + 10 * closeNods.Count;
                        if (bstFst == true)
                        {
                            gCost = 0;
                        }
                        float fCost = gCost + hCost;
                        if (fCostMin > fCost)
                        {
                            fCostMin = fCost;
                            iMn2 = iMn;
                            iMn = ng;
                        }
                    }
                }
                if (iMn < 0)
                {
                    {
                        resultList2.Clear();
                    }
                    break;
                }
                closeNods.Add(iMn);
                curNo = iMn;
                if (curNo == dstNo)
                    break;
            }
            for (int i = 1; i < closeNods.Count; i++)
            {
                int n1 = (int)closeNods[i - 1];
                int n2 = (int)closeNods[i];
                if (bstFst == true)
                {
                    PlotSrcDst(bm, n1, Brushes.Maroon, n2, Brushes.Maroon, Pens.MediumSeaGreen, false);
                    bfs = closeNods.Count - 2;
                }
                else
                {
                    PlotSrcDst(bm, n1, Brushes.Pink, n2, Brushes.Pink, Pens.Yellow, false);
                    astr = closeNods.Count - 2;
                }
            }
        }
        //***********************************************************
        private int[] GetNodeSrtdDistList(ArrayList nod2dNew, int dstNo)
        {
            int[] dist = new int[nod2dNew.Count];
            string[] dt = (string[])nod2dNew[dstNo];
            ArrayList al = new ArrayList();
            string[] wrd1 = dt[0].Split(',');
            string[] wrd2 = dt[1].Split(',');
            al.Add(dstNo.ToString().PadLeft(4, '0') + ",0");
            for (int i = 0; i < wrd1.Length; i++)
            {
                al.Add(wrd1[i].PadLeft(4, '0') + "," + wrd2[i]);
            }
            al.Sort();
            for (int i = 0; i < al.Count; i++)
            {
                dist[i] = int.Parse(al[i].ToString().Split(',')[1]);
            }
            return dist;
        }
        //***********************************************************
        private int[] Str2IntArr(string[] nuStr)
        {
            int[] nu = new int[nuStr.Length];
            for (int i = 0; i < nuStr.Length; i++)
            {
                nu[i] = int.Parse(nuStr[i]);
            }
            return nu;
        }
        //***********************************************************
        private int GetNg(int ng1, ArrayList pathNos)
        {
            if (ng1 != dstNo)
                for (int i = 0; i < pathNos.Count; i++)
                {
                    if (ng1 == (int)pathNos[i])
                    {
                        return -1;
                    }
                }
            return ng1;
        }
        //**********************************************************
        private void btnReDraw_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < nodSz.Length; i++)
            {
                nodSz[i] = 1.0f;
            }
            DrawNodes(true);
            Brush srcBrs = Brushes.Green;
            Brush dstBrs = Brushes.Red;
            Pen linePen = Pens.Red;
            PlotSrcDst(bm, srcNo, srcBrs, dstNo, dstBrs, linePen, true);//true
            if (chkbxObst.Checked)
            {
                DrawObstacle();
            }
        }
        //**********************************************************
        private void chkbxGrid_CheckedChanged(object sender, EventArgs e)
        {
            DrawNodes(true);
        }
        //**********************************************************
        private void lblSrcDst_Click(object sender, EventArgs e)
        {
            //Ok
        }
        //**********************************************************
        private void SavePolyList(ArrayList polyList, string flnm)
        {
            StreamWriter sw = new StreamWriter(flnm);
            for (int i = 0; i < polyList.Count; i++)
            {
                PointF[] poly2 = (PointF[])polyList[i];
                string line = "";
                for (int j = 0; j < poly2.Length; j++)
                {
                    PointF pt = poly2[j];
                    line += pt.X.ToString() + "," + pt.Y.ToString() + ",";
                }
                line = line.TrimEnd(',');
                sw.WriteLine(line);
            }
            sw.Close();
        }
        //**********************************************************
        private void DrawObstacle()
        {
            polyList = LoadPolyList("PolyList.txt");
            bm = (Bitmap)pictureBox1.Image;
            Graphics g = Graphics.FromImage(bm);
            //g.Clear(Color.Blue);
            for (int i = 0; i < polyList.Count; i++)
            {
                PointF[] poly2 = (PointF[])polyList[i];
                g.FillPolygon(Brushes.Red, poly2);
            }
            g.Dispose();
            pictureBox1.Image = bm;
        }
        //**********************************************************
        private ArrayList LoadPolyList(string flnm)
        {
            ArrayList al = new ArrayList();
            if (!File.Exists(flnm))
                return al;
            StreamReader sr = new StreamReader(flnm);
            for (; ; )
            {
                string line = sr.ReadLine();
                if (line == null)
                    break;
                string[] wrds = line.Split(',');
                int n = 0;
                PointF[] pts = new PointF[wrds.Length / 2];
                for (int i = 0; i < pts.Length; i++)
                {
                    float x = float.Parse(wrds[n++]);
                    float y = float.Parse(wrds[n++]);
                    pts[i] = new PointF(x, y);
                }
                al.Add(pts);
            }
            sr.Close();
            return al;
        }
        //**********************************************************
        private void chkbxObst_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbxObst.Checked)
            {
                float obsSz = float.Parse(txbxObst.Text) / 100.0f;
                if (obsSz > 1.0f)
                    txbxObst.Text = "100";
                float margin = (1.0f - obsSz) / 2.0f;
                polyList = new ArrayList();
                PointF[] poly2 = new PointF[4];
                poly2[0] = new PointF(bm.Width / 2 - 5, bm.Height * margin);
                poly2[1] = new PointF(bm.Width / 2 + 5, bm.Height * margin);
                poly2[2] = new PointF(bm.Width / 2 + 5, bm.Height * (1 - margin));
                poly2[3] = new PointF(bm.Width / 2 - 5, bm.Height * (1 - margin));
                polyList.Add(poly2);
                SavePolyList(polyList, "PolyList.txt");
                int[] nds = IsNodesInsidePoly();
                if (nds.Length > 0)
                {
                    for (int i = 0; i < nds.Length; i++)
                    {
                        if (rnd.Next(2) == 0)
                            nods[nds[i]].X += 15;
                        else
                            nods[nds[i]].X -= 15;
                    }
                }
                btnReDraw_Click(null, null);
                DrawObstacle();
                
                if (obstFlg == false)
                {
                    PopulateNode2D();
                    btnReDraw_Click(null, null);
                }
            }
            else
            {
                btnReDraw_Click(null, null);
            }
        }
        //**********************************************************
        private int[] IsNodesInsidePoly()
        {
            ArrayList polLst = LoadPolyList("PolyList.txt");
            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.Blue);
            for (int i = 0; i < polLst.Count; i++)
            {
                PointF[] poly2 = (PointF[])polyList[i];
                g.FillPolygon(Brushes.Red, poly2);
            }
            g.Dispose();
            ArrayList al = new ArrayList();
            for (int j = 0; j < nods.Length; j++)
            {
                Color col = bmp.GetPixel((int)nods[j].X, (int)nods[j].Y);
                if ((col.R == Color.Red.R) & (col.G == Color.Red.G) & (col.B == Color.Red.B))
                {
                    al.Add(j);
                }
            }
            int[] dt = new int[al.Count];
            for (int i = 0; i < al.Count; i++)
            {
                dt[i] = (int)al[i];
            }
            return dt;
        }
        //**********************************************************
        private void btnDoAll_Click(object sender, EventArgs e)
        {
            if (btnDoAll.Text == "DoAll-Start")
            {
                btnDoAll.Text = "DoAll-Stop";
                totalMsg = 0;
                loop = 0;
                timer1.Interval = 100;
                timer1.Tag = "Start";
                timer1.Start();
            }
            else
            {
                btnDoAll.Text = "DoAll-Start";
                timer1.Tag = "";
                timer1.Stop();
            }
        }
        //**********************************************************
        private void timer1_Tick(object sender, EventArgs e)
        {
            btnSrcDst_Click(null, null);
            btnAStar_Click(null, null);
            btnBstFst_Click(null, null);
            pictureBox1.Refresh();
            if (bfs > astr)
            {
                sd = new int[2];
                sd[0] = srcNo;
                sd[1] = dstNo;
                resultList3.Add(sd);
                if (resultList3.Count >= 10)
                {
                    timer1.Stop();
                    btnDoAll.Text = "DoAll-Start";
                }
            }
            totalMsg++;
            lblTotalMsg.Text = totalMsg.ToString();
            loop++;
            if (loop >= 1000)
            {
                timer1.Stop();
                btnDoAll.Text = "DoAll-Start";
            }
        }
        //**********************************************************
        private void lblSrcDst_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                srcNo = -1;
                dstNo = -1;
                DrawNodes(true);
                DrawObstacle();
                lblSrcDst.Text = "";
            }
            else
            {
                if (srcNo >= 0)
                {
                    int no = srcNo;
                    srcNo = dstNo;
                    dstNo = no;
                    DrawNodes(true);
                    //DrawObstacle();
                    lblSrcDst.Text = srcNo.ToString() + "," + dstNo.ToString();
                    GetSrcDst(true);
                    lblBFCnt.Text = "";
                    lblAstarCnt.Text = "";
                }
            }
        }
        //**********************************************************
        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveParameters();
            Experiments();
        }
        //**********************************************************
        private void SaveParameters()
        {
            StreamWriter sw = new StreamWriter("Parameters.txt");
            sw.WriteLine(nods.Length.ToString());//max nodes
            for (int i = 0; i < nods.Length; i++)
            {//Save nodes
                PointF pt = nods[i];
                sw.WriteLine(pt.X.ToString() + "," + pt.Y.ToString());
            }
            for (int i = 0; i < nod2dNew.Count; i++)
            {//Save nebours
                string[] limes3 = (string[])nod2dNew[i];
                for (int j = 0; j < limes3.Length; j++)
                {
                    sw.WriteLine(limes3[j]);
                }
            }
            sw.WriteLine(chkbxObst.Checked.ToString());//Save if obsticle checked
            if (chkbxObst.Checked)
            {//Save Obstles
                sw.WriteLine(polyList.Count.ToString());
                for (int i = 0; i < polyList.Count; i++)
                {
                    PointF[] poly2 = (PointF[])polyList[i];
                    sw.WriteLine(poly2.Length.ToString());
                    for (int j = 0; j < poly2.Length; j++)
                    {
                        sw.WriteLine(poly2[j].X.ToString() + "," + poly2[j].Y.ToString());
                    }
                }
            }
            sw.Close();
        }
        //**********************************************************
        private void SaveHistory()
        {
            //srcNo, dstNo, 2, ASNodes, BSNodes 
            sw0 = new StreamWriter("Experiments.txt");
            string s = "ABflag,srcNo,dstNo,2,ASNodes,BSNodes";
            sw0.WriteLine(s);
            for (int i = 0; i < anlysis.Count; i++)
            {
                sw0.WriteLine(anlysis[i]);
            }
            sw0.Close();
            
        }
        //**********************************************************
        private void LoadParameters()
        {
            StreamReader sr = new StreamReader("Parameters.txt");
            int nodeMx = int.Parse(sr.ReadLine());
            nods = new PointF[nodeMx];
            textBox1.Text = nodeMx.ToString();
            for (int i = 0; i < nods.Length; i++)
            {
                string[] wrds = sr.ReadLine().Split(',');
                nods[i] = new PointF(float.Parse(wrds[0]), float.Parse(wrds[1]));
            }
            nod2dNew = new ArrayList();
            for (int i = 0; i < nods.Length; i++){
                string[] limes3 = new string[3];
                limes3[0] = sr.ReadLine();
                limes3[1] = sr.ReadLine();
                limes3[2] = sr.ReadLine();
                nod2dNew.Add(limes3);
            }
            string poltFlg = sr.ReadLine();
            chkbxObst.Checked = false;
            if (poltFlg == "True")
            {
                chkbxObst.Checked = true;
                polyList = new ArrayList();
                int polyMx = int.Parse(sr.ReadLine());
                for (int i = 0; i < polyMx; i++)
                {
                    int nos = int.Parse(sr.ReadLine());
                    PointF[] pt4 = new PointF[nos];
                    for (int j = 0; j < nos; j++)
                    {
                        string[] wrds = sr.ReadLine().Split(',');
                        pt4[j] = new PointF(float.Parse(wrds[0]), float.Parse(wrds[1]));
                    }
                    polyList.Add(pt4);
                }
            }
            sr.Close();
        }
        //**********************************************************
        private void Experiments()
        {
            countMx = int.Parse(txbxSave.Text);
            count2 = 0;
            timer2.Interval = 1;
            timer2.Start();
        }
        //**********************************************************
        private void timer2_Tick(object sender, EventArgs e)
        {
            count2++;
            txbxSave.Text = count2.ToString();
            btnSrcDst_Click(null,null);
            btnAStar_Click(null, null);
            btnBstFst_Click(null, null);
            string s = "";
            if (astr < bfs)
                s = "" + '0' + ',' + srcNo.ToString() + ',' + dstNo.ToString() + ',' + '2' + ',' + astr.ToString() + ',' + bfs.ToString();
            else
                s = "" + '1' + ',' + srcNo.ToString() + ',' + dstNo.ToString() + ',' + '2' + ',' + astr.ToString() + ',' + bfs.ToString();
            anlysis.Add(s);
            if (count2 >= countMx)
            {
                timer2.Stop();
                txbxSave.Text = countMx.ToString();
                anlysis.Sort();
                SaveHistory();
            }
        }
        //**********************************************************
        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadParameters();
            DrawNodes(true);
            chkbxObst_CheckedChanged(null, null);
        }
        //**********************************************************
        private void btnAnalysis_Click(object sender, EventArgs e)
        {
            btnClear_Click(null, null);
            btnLoad_Click(null, null);
            string[] lines = File.ReadAllLines("Experiments.txt");
            for (int i = 1; i <lines.Length; i++)
            {
                string[] wrds = lines[i].Split(',');
                if (wrds[0] == "0")
                    cmbxAnalysis.Items.Add(wrds[1] + ',' + wrds[2]);
            }
            cmbxAnalysis.SelectedIndex = 0;
        }
        //**********************************************************
        private void cmbxAnalysis_SelectedIndexChanged(object sender, EventArgs e)
        {
            srcNo = int.Parse(cmbxAnalysis.SelectedItem.ToString().Split(',')[0]);
            dstNo = int.Parse(cmbxAnalysis.SelectedItem.ToString().Split(',')[1]);
            PlotSrcDst(bm, srcNo, Brushes.Green, dstNo, Brushes.Red, Pens.Red, true);//true
            btnAStar_Click(null, null);
            btnBstFst_Click(null, null);
            lblSrcDst.Text = srcNo.ToString() + "," + dstNo.ToString();
        }
        //**********************************************************
        private void timer3_Tick(object sender, EventArgs e)
        {
            float nd = 1 + d30;
            float nd2 = nd / 2;
            Graphics g = Graphics.FromImage(bm);
            g.FillEllipse(Brushes.Black, nods[count3].X - nd2, nods[count3].Y - nd2, nd, nd);
            g.Dispose();
            pictureBox1.Image = bm;
            string[] st = ReturnNeibours(count3);
            nod2dNew.Add(st);
            textBox1.Text = count3.ToString();
            count3++;
            if (count3 >= nods.Length)
            {
                count3 = 0;
                timer3.Stop();
                textBox1.Text = nods.Length.ToString();
                btnReDraw_Click(null, null);
            }
        }
        //**********************************************************
        private void btnHelp_Click(object sender, EventArgs e)
        {
            HelpDoc helpdoc = new HelpDoc();
            helpdoc.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        //**********************************************************
    }
}
