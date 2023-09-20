using Maui.GoogleMaps;
using MapUtilsBinding;
using Google.Maps;


namespace MauiBugiOSInteropPerformance.Platforms.iOS.Map
{
    public class ClusterLogic
    {
        private MapView nativeMap;       
        private ClusterRendererHandler clusterRenderer;
        private ClusterManager clusterManager;
        private static IClusterAlgorithm algorithm;
        private static bool itemAddedToAlgorithm = false;

        public ClusterLogic(MapView nativeMap, ClusterOptions clusterOptions)
        {
            this.nativeMap=nativeMap;            
            if (nativeMap == null) { return; }
           
            if (algorithm == null)
                algorithm = GetClusterAlgorithm();

            var iconGenerator = new ClusterIconGeneratorHandler(clusterOptions);
            clusterRenderer = new ClusterRendererHandler(nativeMap, iconGenerator,
                clusterOptions.MinimumClusterSize);
            clusterManager = new ClusterManager(nativeMap, algorithm, clusterRenderer);
            nativeMap.TappedMarker = HandleGmsTappedMarker;
          
           
            //var iconGenerator = new ClusterIconGeneratorHandler(ClusteredMap.ClusterOptions);
            //clusterRenderer = new ClusterRendererHandler(newNativeMap, iconGenerator,
            //    ClusteredMap.ClusterOptions.MinimumClusterSize);


            //ClusteredMap.OnCluster = HandleClusterRequest;
            //ClusteredMap.OnAddClusterItems = HandleAddingCluteredItems;
            //ClusteredMap.OnMarkerIconChange = HandleMarkerIconChange;

            //if (newNativeMap == null) return;
            //newNativeMap.InfoTapped += OnInfoTapped;
            //newNativeMap.InfoLongPressed += OnInfoLongPressed;
            //newNativeMap.TappedMarker = HandleGmsTappedMarker;
            //newNativeMap.InfoClosed += InfoWindowClosed;
            //newNativeMap.DraggingMarkerStarted += DraggingMarkerStarted;
            //newNativeMap.DraggingMarkerEnded += DraggingMarkerEnded;
            //newNativeMap.DraggingMarker += DraggingMarker;
        }

        public void CameraIdled()
        {
            clusterManager.Cluster();
        }

        public void ChangeClusterItemIcon(ClusterMarker marker, string iconName)
        {
            clusterRenderer.ChangeMarkerIcon(marker, iconName);
        }

        private bool HandleGmsTappedMarker(MapView mapView, Marker marker)
        {
            if (marker?.UserData is ICluster cluster)
            {
                var pins = GetClusterPins(cluster);
                var clusterPosition = new Position(cluster.Position.Latitude, cluster.Position.Longitude);
               
                return true;

            }
            var targetPin = marker.UserData as IosClusterItem;

            if (targetPin != null)
            {                
               
                return true;                
            }
            return false;
        }

        private List<ClusterMarker> GetClusterPins(ICluster cluster)
        {
            var pins = new List<ClusterMarker>();
            foreach (var item in cluster.Items)
            {
                var clusterItem = (IosClusterItem)item;
                pins.Add(clusterItem.Item);
            }

            return pins;
        }



        public void ClusterItems(List<ClusterMarker> list)
        {
            if (algorithm != null && itemAddedToAlgorithm == true)
            {
                clusterManager.ClearItems();
            }
            if (list == null || list.Count == 0)
            {
                itemAddedToAlgorithm = false;
                return;
            }
                
            var newList = new List<IosClusterItem>();
            foreach (var item in list)
            {
                var ci = new IosClusterItem(item);
                newList.Add(ci);
            }
            itemAddedToAlgorithm = true;
            clusterManager.AddItems(newList.ToArray());
            clusterManager.Cluster();
        }
        

        private IClusterAlgorithm GetClusterAlgorithm()
        {
            var algorithm  = new NonHierarchicalDistanceBasedAlgorithm();            

            return algorithm;
        }
    }
}
