using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Windows.Foundation;
using Windows.Devices.Geolocation;
using System.Windows.Threading;
using UnityApp = UnityPlayer.UnityApp;
using UnityBridge = WinRTBridge.WinRTBridge;
using vservWindowsPhone;//Full Ads
using InMobi.WP.AdSDK;
using GoogleAds;

namespace JewelsBlup
{
	public partial class MainPage : PhoneApplicationPage
	{
		private bool _unityStartedLoading;
		private bool _useLocation;
        private InterstitialAd interstitialAd;
        AdView bannerAd;
        public static int isShowAds = 1;//1 = true
		// Constructor
		public MainPage()
		{
			var bridge = new UnityBridge();
			UnityApp.SetBridge(bridge);
			InitializeComponent();
			bridge.Control = DrawingSurfaceBackground;
            VservAdControl VMB = VservAdControl.Instance; //full ads
            VMB.DisplayAd("39a99e62", LayoutRoot);  //n_m_hoang
            VMB.VservAdClosed += new EventHandler(VACCallback_OnVservAdClosing);
            VMB.VservAdError += new EventHandler(VACCallback_OnVservAdNetworkError);
            VMB.VservAdNoFill += new EventHandler(VACCallback_OnVservAdNoFill);
		}

      void VACCallback_OnVservAdClosing(object sender, EventArgs e)
        {
         
            
        }
        void VACCallback_OnVservAdNetworkError(object sender, EventArgs e)
        {
        }

        void VACCallback_OnVservAdNoFill(object sender, EventArgs e)
        {
            interstitialAd = new InterstitialAd("ca-app-pub-6844968633010430/7179482305");
            AdRequest adRequest = new AdRequest();
            interstitialAd.ReceivedAd += OnAdReceived;
           //adRequest.ForceTesting = true;//here to rem
            interstitialAd.LoadAd(adRequest);
        }
        private void OnAdReceived(object sender, AdEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Ad received successfully");            
            interstitialAd.ShowAd();
        }
		private void DrawingSurfaceBackground_Loaded(object sender, RoutedEventArgs e)
		{
			if (!_unityStartedLoading)
			{
				_unityStartedLoading = true;

				UnityApp.SetLoadedCallback(() => { Dispatcher.BeginInvoke(Unity_Loaded); });

				var content = Application.Current.Host.Content;
				var width = (int)Math.Floor(content.ActualWidth * content.ScaleFactor / 100.0 + 0.5);
				var height = (int)Math.Floor(content.ActualHeight * content.ScaleFactor / 100.0 + 0.5);

				UnityApp.SetNativeResolution(width, height);
				UnityApp.SetRenderResolution(width, height);
				UnityPlayer.UnityApp.SetOrientation((int)Orientation);

				DrawingSurfaceBackground.SetBackgroundContentProvider(UnityApp.GetBackgroundContentProvider());
				DrawingSurfaceBackground.SetBackgroundManipulationHandler(UnityApp.GetManipulationHandler());

                bannerAd = new AdView
                {
                    Format = AdFormats.Banner,
                    AdUnitID = "ca-app-pub-6844968633010430/8656215506"
                };
              
                bannerAd.FailedToReceiveAd += OnFailedToReceiveAd;
                AdRequest adRequest = new AdRequest();                
                //adRequest.ForceTesting = true;//here rem here
                adGridAdmob.Children.Add(bannerAd);
                //bannerAd.LoadAd(adRequest);
                toanstt_Refresh_admob();
            }
		}
        private void OnFailedToReceiveAd(object sender, AdErrorEventArgs errorCode)
        {
             //  adGridAdmob.Visibility = Visibility.Collapsed;
            //if (isShowAds == 1)
            //    AdsManager.showAds(DrawingSurfaceBackground, AdsManager.INDEX_INMOBI);
            // else
            //    AdsManager.showAds(DrawingSurfaceBackground, AdsManager.INDEX_INNER_ACTIVE);	
        }

        void OnTimerTick1(Object sender, EventArgs args)
        {            
            AdRequest adRequest = new AdRequest();
            //adRequest.ForceTesting = true;//here rem here
            bannerAd.LoadAd(adRequest);
            
        }        
        void toanstt_Refresh_admob()
        {
            DispatcherTimer newTimer = new DispatcherTimer();
            newTimer.Interval = TimeSpan.FromSeconds(60);
            newTimer.Tick += OnTimerTick1;
            newTimer.Start();
        }
       
		private void Unity_Loaded()
		{
			SetupGeolocator();
		}

		private void PhoneApplicationPage_BackKeyPress(object sender, CancelEventArgs e)
		{
			e.Cancel = UnityApp.BackButtonPressed();
		}

		private void PhoneApplicationPage_OrientationChanged(object sender, OrientationChangedEventArgs e)
		{
			UnityApp.SetOrientation((int)e.Orientation);
		}

		protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (!UnityApp.IsLocationEnabled())
                return;
            if (IsolatedStorageSettings.ApplicationSettings.Contains("LocationConsent"))
                _useLocation = (bool)IsolatedStorageSettings.ApplicationSettings["LocationConsent"];
            else
            {
                MessageBoxResult result = MessageBox.Show("Can this application use your location?",
                    "Location Services", MessageBoxButton.OKCancel);
                _useLocation = result == MessageBoxResult.OK;
                IsolatedStorageSettings.ApplicationSettings["LocationConsent"] = _useLocation;
                IsolatedStorageSettings.ApplicationSettings.Save();
            }
        }

		private void SetupGeolocator()
        {
            if (!_useLocation)
                return;

            try
            {
				UnityApp.EnableLocationService(true);
                Geolocator geolocator = new Geolocator();
				geolocator.ReportInterval = 5000;
                IAsyncOperation<Geoposition> op = geolocator.GetGeopositionAsync();
                op.Completed += (asyncInfo, asyncStatus) =>
                    {
                        if (asyncStatus == AsyncStatus.Completed)
                        {
                            Geoposition geoposition = asyncInfo.GetResults();
                            UnityApp.SetupGeolocator(geolocator, geoposition);
                        }
                        else
                            UnityApp.SetupGeolocator(null, null);
                    };
            }
            catch (Exception)
            {
                UnityApp.SetupGeolocator(null, null);
            }
        }
	}
}