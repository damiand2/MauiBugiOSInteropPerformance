using Google.Maps;
using Maui.GoogleMaps;
using MauiBugiOSInteropPerformance.Platforms.iOS.Map;
using Microsoft.Maui.Platform;

namespace MauiBugiOSInteropPerformance
{
    public partial class MainPage : ContentPage
    {
        
        private ClusterLogic clusterLogic;
        public MainPage()
        {
            InitializeComponent();
           
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var position = new Position(49.804, 15.475);
            var latestMapRegion = new MapSpan(position, 2.504, 6.769);

            map.MoveToRegion(latestMapRegion);
            map.CameraIdled +=Map_CameraIdled;
        }

        private void Map_CameraIdled(object sender, CameraIdledEventArgs e)
        {
            
            map.CameraIdled -=Map_CameraIdled;
            clusterLogic = new ClusterLogic((MapView)map.Handler.PlatformView, new ClusterOptions());
            List<ClusterMarker> markers = new List<ClusterMarker>();
            for (int i = 0; i < 2000; i++)
            {
                markers.Add(new ClusterMarker(
                    i.ToString(),
                    i.ToString(),
                    i % 2 ==0 ? 49.804 + Random.Shared.NextDouble() : 49.804 - Random.Shared.NextDouble(),
                    i % 2 ==0 ? 15.475 + (Random.Shared.NextDouble() * 2) : 15.475 - (Random.Shared.NextDouble() * 2),
                    "icpin"
                ));
            }
            clusterLogic.ClusterItems(markers);
        }
    }
}