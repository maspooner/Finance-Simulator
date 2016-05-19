﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FinanceSim {
	public partial class MainWindow : Window {
		//members
		private TitleView titleView;
		private ProfileView profileView;
		private DataView dataView;
		private List<Profile> profiles;
		//constructors
		public MainWindow() {
			InitializeComponent();
			titleView = new TitleView(this);
			profileView = new ProfileView(this);
			dataView = new DataView(this);
			profiles = LoadProfiles();
			titleView.OpenProfiles(profiles);
			Content = titleView;
		}
		//methods
		internal void OpenProfile(int i) {
			OpenProfile(profiles[i]);
		}
		internal void OpenProfile(Profile p) {
			dataView.OpenProfile(p);
			Content = dataView;
		}
		private void SaveProfiles() {
			profiles.Sort();
			using (Stream stream = File.Open(Environment.CurrentDirectory + "/financeData.bin", FileMode.Create)) {
				BinaryFormatter bformatter = new BinaryFormatter();
				bformatter.Serialize(stream, profiles);
			}
		}
		internal List<Profile> LoadProfiles() {
			List<Profile> profiles;
			FileInfo info = new FileInfo(Environment.CurrentDirectory + "/financeData.bin");
			if (info.Exists) {
				using (Stream stream = File.Open(Environment.CurrentDirectory + "/financeData.bin", FileMode.Open)) {
					BinaryFormatter bformatter = new BinaryFormatter();
					profiles = bformatter.Deserialize(stream) as List<Profile>;
				}
			}
			else {
				profiles = new List<Profile>();
			}
			return profiles;
		}
		internal void AddProfile(Profile profile) {
			profiles.Add(profile);
			SaveProfiles();
		}
		internal void ReturnAndSave() {
			SaveProfiles();
			titleView.OpenProfiles(profiles);
			Return();
		}
		internal void Return() {
			Content = titleView;
		}
		internal void NewProfile() {
			profileView.NewProfile();
			Content = profileView;
		}
		internal void EditProfile(Profile profile) {
			profileView.LoadProfile(profile);
			Content = profileView;
		}
		internal void EditProfile(int i) {
			EditProfile(profiles[i]);
		}
	}
}
