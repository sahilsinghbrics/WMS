// ----------------------------------------------------------------------------------
// Copyright © 2018, Oleg Lobakov, Contacts: <oleg.lobakov@gmail.com>
// Licensed under the GNU GENERAL PUBLIC LICENSE, Version 3.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// https://github.com/OlegLobakov/WarehouseControlSystem/blob/master/LICENSE
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WarehouseControlSystem.ViewModel;

namespace WarehouseControlSystem.View.Content
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class IndicatorView : ContentView
	{

        public static readonly BindableProperty WidthHeightProperty = BindableProperty.Create("WidthHeight", typeof(decimal), typeof(IndicatorView), (decimal)200);
        public decimal WidthHeight
        {
            get { return (decimal)GetValue(WidthHeightProperty); }
            set { SetValue(WidthHeightProperty, value); }
        }

        public IndicatorView()
        {
            InitializeComponent();
        }

        bool tapkey = true;

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (tapkey)
            {
                tapkey = false;
                await this.FadeTo(0.7, 500, Easing.SinInOut);
                await this.FadeTo(1, 500, Easing.SinInOut);
                tapkey = true;
            }
        }
    }
}