using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace xMeans
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private int PlotDrawCount = 0;

		private void Form1_Load(object sender, EventArgs e)
		{
			chart1.Series.Clear();
		}

		private void ReadFileButton_Click(object sender, EventArgs e)
		{
			var points = new List<Point>();

			//File Dialog
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "(*.txt)|*.txt";
			ofd.FilterIndex = 2;
			ofd.Title = "Select 2D Data";
			ofd.RestoreDirectory = true;

			if (ofd.ShowDialog() == DialogResult.OK)
			{
				var path = ofd.FileName;
				var lines = File.ReadAllLines(path);
				foreach (var line in lines)
				{
					var sp = line.Split(',');
					points.Add(new Point(int.Parse(sp[0]), int.Parse(sp[1])));
				}
				ActMain(points);
			}
		}

		private void RndBtn_Click(object sender, EventArgs e)
		{
			var rndPoints = GetRndPoint(50);
			ActMain(rndPoints);
		}

		private void ActMain(List<Point> points)
		{
			PlotDrawCount = 0;
			chart1.Series.Clear();

			//kemans draw
			/*
			var k = 4;
			var kmeans = new Kmeans(k, points);
			var loopCount = kmeans.Calculation(1);
			DrawGraph(kmeans);
			*/

			//xmenas draw

			var xMeans = new Xmeans(points);
			var kmeansResults = xMeans.Calculation();
			var cSum = kmeansResults.Sum(x => x.ClusterPoints.Count()); //All Point Number

			//Console.WriteLine("{0}", cSum); //debug

			foreach (var mean in kmeansResults)
				DrawGraph(mean);
		}

		private List<ClusteringPoint> Kmeans(int k, List<Point> points)
		{
			var cList = new List<ClusteringPoint>();
			var rnd = new Random();
			foreach (var p in points)
			{
				var cluster = rnd.Next(k);
				cList.Add(new ClusteringPoint(p, cluster));
			}

			return cList;
		}

		private void DrawGraph(Kmeans kmeans)
		{
			var plotName = "p" + PlotDrawCount;

			//Init
			for (int i = 0; i < kmeans.K; i++)
			{
				if (kmeans.CenterList[i].X != int.MaxValue)
				{
					chart1.Series.Add(plotName + i);
					chart1.Series[plotName + i].IsVisibleInLegend = false;
					chart1.Series[plotName + i].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
				}
			}

			//plot Normal Point
			foreach (var p in kmeans.ClusterPoints)
			{
				chart1.Series[plotName + p.ClusterIndex].Points.AddXY(p.Point.X, p.Point.Y);
			}

			//Clusterling Average Plot
			for (int i = 0; i < kmeans.CenterList.Count; i++)
			{
				if (kmeans.CenterList[i].X != int.MaxValue)
				{
					chart1.Series[plotName + i].Points.AddXY(kmeans.CenterList[i].X, kmeans.CenterList[i].Y);
					var len = chart1.Series[plotName + i].Points.Count;
					chart1.Series[plotName + i].Points[len - 1].MarkerSize = 15;
				}
			}
			PlotDrawCount++;
		}

		private List<Point> GetRndPoint(int num)
		{
			var list = new List<Point>();
			var rnd = new System.Random();
			for (var i = 0; i < num; i++)
				list.Add(new Point((int)rnd.Next(100), (int)rnd.Next(100)));
			return list;
		}
	}

	public class ClusteringPoint
	{
		public int ClusterIndex;
		public Point Point;

		public ClusteringPoint(Point p, int cluserIndex)
		{
			ClusterIndex = cluserIndex;
			Point = p;
		}
	}

	public class Xmeans
	{
		public Kmeans TopKm;
		private int _k = 2;

		public Xmeans(List<Point> points)
		{
			TopKm = new Kmeans(_k, points);
		}

		private List<Kmeans> RecursiveProcessing(Kmeans parent, List<Kmeans> register)
		{
			var child1 = new Kmeans(_k, parent.ClusterPoints.Where(x => x.ClusterIndex == 0).Select(x => x.Point).ToList());
			var child2 = new Kmeans(_k, parent.ClusterPoints.Where(x => x.ClusterIndex == 1).Select(x => x.Point).ToList());
			child1.Calculation(1);
			child2.Calculation(1);
			if (parent.CenterDistDispersion[0] > child1.CenterDistDispersion[0] + child1.CenterDistDispersion[1]
				//	if( parent.BICs[0] > child1.BICs[0] + child1.BICs[1]
				&& child1.ClusterPoints.Count > 1)
			{
				RecursiveProcessing(child1, register);
			}
			else register.Add(child1);
			if (parent.CenterDistDispersion[1] > child2.CenterDistDispersion[0] + child2.CenterDistDispersion[1]
				//	if(parent.BICs[1] > child2.BICs[0] + child2.BICs[1]
				&& child2.ClusterPoints.Count > 1)
			{
				RecursiveProcessing(child2, register);
			}
			else register.Add(child2);

			return register;
		}

		public List<Kmeans> Calculation()
		{
			TopKm.Calculation(1);
			var src = new List<Kmeans>();
			var result = RecursiveProcessing(TopKm, src);
			return result;
		}
	}

	public class Kmeans
	{
		public int K;
		public List<ClusteringPoint> ClusterPoints = new List<ClusteringPoint>();
		public List<Point> CenterList = new List<Point>();
		public List<Point> BeforeCenterList = new List<Point>();
		public List<double> CenterDistAvg = new List<double>();
		public List<double> CenterDistDispersion = new List<double>();

		public List<double> BICs = new List<double>();

		public Kmeans(int k, List<Point> points)
		{
			K = k;
			var rnd = new Random();
			foreach (var p in points)
			{
				//First is random
				var cluster = rnd.Next(k);
				ClusterPoints.Add(new ClusteringPoint(p, cluster));
			}

			for (int i = 0; i < K; i++)
			{
				CenterDistAvg.Add(int.MaxValue);
				CenterDistDispersion.Add(int.MaxValue);
				BICs.Add(int.MaxValue);
				CenterList.Add(new Point(int.MaxValue, int.MaxValue));
			}

			BeforeCenterList = new List<Point>(CenterList);
		}

		private void OneAct()
		{
			//Update Center
			for (int i = 0; i < K; i++)
			{
				BeforeCenterList[i] = new Point(CenterList[i].X, CenterList[i].Y);

				var cv = ClusterPoints.Where(x => x.ClusterIndex == i);
				if (cv.Count() > 0)
				{
					var avgX = cv.Average(x => x.Point.X);
					var avgY = cv.Average(x => x.Point.Y);
					CenterList[i] = new Point((int)avgX, (int)avgY);
				}
				//error exception
				else
				{
					BeforeCenterList[i] = new Point(int.MaxValue, int.MaxValue);
					CenterList[i] = new Point(int.MaxValue, int.MaxValue);
				}
			}

			//Update Nearest Cluster
			foreach (var p in ClusterPoints)
			{
				var updateIndex = GetNearestIndex(p.Point, CenterList);
				p.ClusterIndex = updateIndex;
			}

			//Update Avg Center Dist
			for (int i = 0; i < K; i++)
			{
				var cPoints = ClusterPoints.Where(x => x.ClusterIndex == i);

				//error exception
				if (cPoints.Count() == 0)
				{
					CenterDistAvg[i] = int.MaxValue;
					CenterDistDispersion[i] = int.MaxValue;
					BICs[i] = int.MaxValue;
					continue;
				}

				var list = new List<double>();
				foreach (var p in cPoints)
				{
					var dist = Math.Sqrt(Math.Pow(CenterList[i].X - p.Point.X, 2) + Math.Pow(CenterList[i].Y - p.Point.Y, 2));
					list.Add(dist);
				}
				CenterDistAvg[i] = list.Average();

				var sum = 0.0;
				foreach (var l in list)
				{
					sum += Math.Pow((l - CenterDistAvg[i]), 2);
				}
				CenterDistDispersion[i] = sum / (double)list.Count;

				//Cal BICs
				//http://stackoverflow.com/questions/15839774/how-to-calculate-bic-for-k-means-clustering-in-r

				//cal witness
				var sumX = cPoints.Sum(x => x.Point.X);
				var sumY = cPoints.Sum(x => x.Point.Y);
				var sumX2 = cPoints.Sum(x => x.Point.X * x.Point.X);
				var sumY2 = cPoints.Sum(x => x.Point.Y * x.Point.Y);
				var squX = sumX2 - (sumX * sumX / (double)cPoints.Count());
				var squY = sumY2 - (sumY * sumY / (double)cPoints.Count());
				var totWitness = squX + squY;

				var m = 2; //point dimension
				var n = cPoints.Count();
				var d = totWitness;
				BICs[i] = d + Math.Log(n) * m * K;
			}
		}

		public int Calculation(double permitErrorDist)
		{
			var loopCount = 0;
			while (true)
			{
				loopCount++;
				OneAct();
				var ok = true;
				//judge all center change value is less than error dist
				for (int i = 0; i < CenterList.Count; i++)
				{
					var dist = Math.Sqrt(Math.Pow(CenterList[i].X - BeforeCenterList[i].X, 2)
									   + Math.Pow(CenterList[i].Y - BeforeCenterList[i].Y, 2));
					if (permitErrorDist < dist)
					{
						ok = false;
						break;
					}
				}
				if (ok) break;
			}
			return loopCount;
		}

		private int GetNearestIndex(Point p, List<Point> centerList)
		{
			var value = Math.Sqrt(Math.Pow(centerList[0].X - p.X, 2) + Math.Pow(centerList[0].Y - p.Y, 2));
			var index = 0;

			for (int i = 1; i < centerList.Count; i++)
			{
				var v = Math.Sqrt(Math.Pow(centerList[i].X - p.X, 2) + Math.Pow(centerList[i].Y - p.Y, 2));
				if (v < value)
				{
					value = v;
					index = i;
				}
			}
			return index;
		}
	}
}