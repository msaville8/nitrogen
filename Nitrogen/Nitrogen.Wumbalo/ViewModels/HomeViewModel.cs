﻿using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Nitrogen.Wumbalo.Models.Home;

namespace Nitrogen.Wumbalo.ViewModels
{
	public class HomeViewModel : Inpc
	{
		/// <summary>
		/// 
		/// </summary>
		public HomeViewModel()
		{
			//Headers.CollectionChanged += Headers_CollectionChanged;

			Headers.Add(new Header { IsSelected = false, Name = "Recents", Tag = "Recents", HeaderType = HeaderType.Recents });
			Headers.Add(new Header { IsSelected = false, Name = "Devices", Tag = "Devices", HeaderType = HeaderType.Devices });
			Headers.Add(new Header { IsSelected = true, Name = "Home", Tag = "Home", HeaderType = HeaderType.Home });
			Headers.Add(new Header { IsSelected = false, Name = "Onyx", Tag = "Onyx", HeaderType = HeaderType.Onyx });
#if DEBUG
			Headers.Add(new Header { IsSelected = false, Name = "Development", Tag = "Development", HeaderType = HeaderType.Development });
#endif

			for (var i = 0; i < new Random().Next(8, 16); i++)
				Recents.Add(new Recent { Name = string.Format("Test {0}", i) });

			Recents.CollectionChanged += RecentsOnCollectionChanged;
			Headers.CollectionChanged += HeadersOnCollectionChanged;
			RecentsOnCollectionChanged(null, null);
		}

		#region Events

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="notifyCollectionChangedEventArgs"></param>
		private void RecentsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
		{
			if (sender != null)
				SetField(ref _recents, sender as ObservableCollection<Recent>, "Recents", true);

			_recentsForXaml.Clear();
			var i = 0;
			var recents = new ObservableCollection<Recent>();
			foreach (var recent in Recents)
			{
				recents.Add(recent);
				i++;

				if (i != 3) continue;

				_recentsForXaml.Add(new ObservableCollection<Recent>(recents));
				recents.Clear();
				i = 0;
			}
			if (recents.Any())
				_recentsForXaml.Add(recents);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="notifyCollectionChangedEventArgs"></param>
		private void HeadersOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
		{
			if (sender != null)
				SetField(ref _headers, sender as ObservableCollection<Header>, "Headers", true);
		}

		#endregion

		/// <summary>
		/// 
		/// </summary>
		public ObservableCollection<Header> Headers
		{
			get { return _headers; }
			set { SetField(ref _headers, value, "Headers"); }
		}
		private ObservableCollection<Header> _headers = new ObservableCollection<Header>();

		/// <summary>
		/// 
		/// </summary>
		public ObservableCollection<Recent> Recents
		{
			get { return _recents; }
		}
		private ObservableCollection<Recent> _recents = new ObservableCollection<Recent>();

		/// <summary>
		/// 
		/// </summary>
		public ObservableCollection<ObservableCollection<Recent>> RecentsForXaml
		{
			get { return new ObservableCollection<ObservableCollection<Recent>>(_recentsForXaml.Reverse()); }
		}
		private readonly ObservableCollection<ObservableCollection<Recent>> _recentsForXaml = new ObservableCollection<ObservableCollection<Recent>>();
	}
}
