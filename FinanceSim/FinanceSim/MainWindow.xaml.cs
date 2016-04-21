﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FinanceSim {
	public partial class MainWindow : Window {
		//members
		private List<Payment> payments;
		private Page[] contents;
		//constructors
		public MainWindow() {
			InitializeComponent();
			contents = new Page[] { new ProfileView(this), new DataView(this) };
			payments = new List<Payment>();
			payments.Add(new UncertainRandomPayment("fee", Frequency.WEEKLY, 40.0m, 5.0m, new Random(), 2, DateTime.Now));
			ChangeContent(0);
		}
		//methods
		internal void ChangeContent(int i) {
			Content = contents[i];
		}
	}
}
