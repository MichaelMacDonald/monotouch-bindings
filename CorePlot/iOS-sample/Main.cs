
using System;
using System.Collections.Generic;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CorePlot;
using System.Drawing;

namespace iOSsample
{
	public class Application
	{
		static void Main (string[] args)
		{
			try {
				UIApplication.Main (args);
			} catch (Exception e){
				Console.WriteLine (e);
			}
		}
	}

	// The name AppDelegate is referenced in the MainWindow.xib file.
	public partial class AppDelegate : UIApplicationDelegate
	{
		CPGraphHostingView host;
		CPXYGraph graph;
		
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			window.MakeKeyAndVisible ();
			
			SetupGraph ();
			SetupAxes ();
			SetupScatterPlots ();

			// Host the graph
			host = new CPGraphHostingView (new RectangleF (10, 40, 310, 310));
			host.HostedGraph = graph;
			window.Add (host);
			window.BackgroundColor = UIColor.Brown;
			
			return true;
		}
		
		void SetupGraph ()
		{
			var theme = CPTheme.ThemeNamed ("Plain Black");
			
			graph = new CPXYGraph (new RectangleF (0, 0, 300, 300), CPScaleType.Linear, CPScaleType.Linear){
				PaddingLeft = 10,
				PaddingRight = 10,
				PaddingTop = 10,
				PaddingBottom = 10,
				Title = "My graph",
				
				TitleTextStyle = new CPTextStyle () {
					FontSize = 18,
					FontName = "Helvetica",
					Color = CPColor.GrayColor
				},
			};
			graph.ApplyTheme (theme);
		}
		
		void SetupAxes ()
		{
			var plotspace = graph.DefaultPlotSpace;
			plotspace.AllowsUserInteraction = true;
			
			var major = new CPLineStyle () {
				LineWidth = .75f,
				LineColor = CPColor.FromGenericGray (0.2f).ColorWithAlphaComponent (.75f)
			};
			
			var minor = new CPLineStyle () {
				LineWidth = .25f,
				LineColor = CPColor.WhiteColor.ColorWithAlphaComponent (0.1f)
			};
			
			var axisSet = (CPXYAxisSet) graph.AxisSet;
			
			// Label x with a fixed interval policy
			var x = axisSet.XAxis;
			x.LabelingPolicy = CPAxisLabelingPolicy.PolicyAutomatic;
			x.MinorTicksPerInterval = 4;
			x.PreferredNumberOfMajorTicks = 8;
			x.MajorGridLineStyle = major;
			x.MinorGridLineStyle = minor;
			x.Title = "X Axis";
			x.TitleOffset = -30;

			// Label y with an automatic label policy. 
    		var y = axisSet.YAxis;
		    y.LabelingPolicy = CPAxisLabelingPolicy.PolicyAutomatic;
		    y.MinorTicksPerInterval = 4;
		    y.PreferredNumberOfMajorTicks = 8;
		    y.MajorGridLineStyle = major;
			y.MinorGridLineStyle = minor;
			y.LabelOffset = 20;
			y.Title = "Y Axis";
			y.TitleOffset = -30;
		}
		
		void SetupScatterPlots ()
		{
		    // Create a plot that uses the data source method
        	var dataSourceLinePlot = new CPScatterPlot () {
				CachePrecision = CPPlotCachePrecision.Double,
				DataLineStyle = new CPLineStyle () {
					LineWidth = 2,
					LineColor = CPColor.GreenColor
				},
				// For Kang, check this out:
				//DataSource = new MySource ()
			};
			graph.AddPlot (dataSourceLinePlot);
		}
		

		// This method is required in iPhoneOS 3.0
		public override void OnActivated (UIApplication application)
		{
		}
	}
	
	public class MySource : CPScatterPlotDataSource {
		List<PointF> data;
		
		public MySource ()
		{
			Random r = new Random ();
			
			data = new List<PointF> ();
			for (int i = 0; i < 100; i++)
				data.Add (new PointF (i * 0.5f, (float)(10 * r.NextDouble () - 5)));
		}
	
		public override int NumberOfRecordsForPlot (CPPlot plot)
		{
			return data.Count;
		}
		
		public override NSNumber NumberForPlot (CPPlot plot, CPPlotField forFieldEnum, int index)
		{
			if (forFieldEnum == CPPlotField.ScatterPlotFieldX)
				return data [index].X;
			else
				return data [index].Y;
		}
		
		public override CPPlotSymbol GetSymbol (CPScatterPlot plot, int recordIndex)
		{
			return CPPlotSymbol.DiamondPlotSymbol;	
		}
	}
}

#if false			
 
    // Set plot delegate, to know when symbols have been touched
        // We will display an annotation when a symbol is touched
    dataSourceLinePlot.delegate = self; 
    dataSourceLinePlot.plotSymbolMarginForHitDetection = 5.0;
    
    // Create a plot for the selection marker
        CPScatterPlot *selectionPlot = [[[CPScatterPlot alloc] init] autorelease];
    selectionPlot.identifier = SELECTION_PLOT;
        selectionPlot.cachePrecision = CPPlotCachePrecisionDouble;
    
    lineStyle = [[dataSourceLinePlot.dataLineStyle mutableCopy] autorelease];
        lineStyle.lineWidth = 3.0;
    lineStyle.lineColor = [CPColor redColor];
    selectionPlot.dataLineStyle = lineStyle;
    
    selectionPlot.dataSource = self;
    [graph addPlot:selectionPlot];
    
    // Auto scale the plot space to fit the plot data
    // Compress ranges so we can scroll
    CPXYPlotSpace *plotSpace = (CPXYPlotSpace *)graph.defaultPlotSpace;
    [plotSpace scaleToFitPlots:[NSArray arrayWithObject:dataSourceLinePlot]];
    CPPlotRange *xRange = plotSpace.xRange;
    [xRange expandRangeByFactor:CPDecimalFromDouble(0.75)];
    plotSpace.xRange = xRange;
    CPPlotRange *yRange = plotSpace.yRange;
    [yRange expandRangeByFactor:CPDecimalFromDouble(0.75)];
    plotSpace.yRange = yRange;
    
    CPPlotRange *globalXRange = [CPPlotRange plotRangeWithLocation:CPDecimalFromDouble(-1.0) length:CPDecimalFromDouble(10.0)];
    plotSpace.globalXRange = globalXRange;
    CPPlotRange *globalYRange = [CPPlotRange plotRangeWithLocation:CPDecimalFromDouble(-5.0) length:CPDecimalFromDouble(10.0)];
    plotSpace.globalYRange = globalYRange;
		}
#endif