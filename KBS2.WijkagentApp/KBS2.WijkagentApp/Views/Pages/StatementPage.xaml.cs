﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KBS2.WijkagentApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KBS2.WijkagentApp.Views.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StatementPage : ContentPage
	{
		public StatementPage (BaseViewModel viewModel)
		{
			InitializeComponent ();
		    BindingContext = viewModel;
		}
	}
}