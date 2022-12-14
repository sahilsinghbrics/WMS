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
using Xamarin.Forms;
using System.Windows.Input;
using System.Threading;
using WarehouseControlSystem.Model;

namespace WarehouseControlSystem.ViewModel.Base
{
    public class BaseViewModel : BindableObject
    {
        public INavigation Navigation { get; set; }

        public string Title
        {
            get { return title; }
            set
            {
                if (title != value)
                {
                    title = value;
                    OnPropertyChanged(nameof(Title));
                }
            }
        } string title;

        public ModelState State
        {
            get { return state1; }
            set
            {
                if (state1 != value)
                {
                    state1 = value;
                    ChangeState(state1);
                    LoadAnimation = state1 == ModelState.Loading;
                    OnPropertyChanged(nameof(State));
                }
            }
        } ModelState state1;

        public bool IsLoadingState
        {
            get { return isloadingstate; }
            set
            {
                if (isloadingstate != value)
                {
                    isloadingstate = value;
                    OnPropertyChanged(nameof(IsLoadingState));
                }
            }
        } bool isloadingstate;
        public bool IsNormalState
        {
            get { return isnormalstate; }
            set
            {
                if (isnormalstate != value)
                {
                    isnormalstate = value;
                    OnPropertyChanged(nameof(IsNormalState));
                }
            }
        } bool isnormalstate;
        public bool IsErrorState
        {
            get { return iserrorstate; }
            set
            {
                if (iserrorstate != value)
                {
                    iserrorstate = value;
                    OnPropertyChanged(nameof(IsErrorState));
                }
            }
        }  bool iserrorstate;
        public bool IsRequestState
        {
            get { return isrequeststate; }
            set
            {
                if (isrequeststate != value)
                {
                    isrequeststate = value;
                    OnPropertyChanged(nameof(IsRequestState));
                }
            }
        } bool isrequeststate;
        public bool IsNoDataState
        {
            get { return isnodatastate; }
            set
            {
                if (isnodatastate != value)
                {
                    isnodatastate = value;
                    OnPropertyChanged(nameof(IsNoDataState));
                }
            }
        } bool isnodatastate;

        public bool IsSelected
        {
            get { return isselected; }
            set
            {
                if (isselected != value)
                {
                    isselected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        } bool isselected;

        public string LoadingText
        {
            get { return loadingtext; }
            set
            {
                if (loadingtext != value)
                {
                    loadingtext = value;
                    OnPropertyChanged(nameof(LoadingText));
                }
            }
        } string loadingtext;
        public bool LoadAnimation
        {
            get { return loadanimation; }
            set
            {
                if (loadanimation != value)
                {
                    loadanimation = value;
                    OnPropertyChanged(nameof(LoadAnimation));
                }
            }
        } bool loadanimation;

        public string InfoText
        {
            get { return infotext; }
            set
            {
                if (infotext != value)
                {
                    infotext = value;
                    OnPropertyChanged(nameof(InfoText));
                }
            }
        } string infotext;
        public string ErrorText
        {
            get { return errortext; }
            set
            {
                if (errortext != value)
                {
                    errortext = value;
                    OnPropertyChanged(nameof(ErrorText));
                }
            }
        } string errortext;
        public string RequestLabelText
        {
            get { return requestlabeltext; }
            set
            {
                if (requestlabeltext != value)
                {
                    requestlabeltext = value;
                    OnPropertyChanged(nameof(RequestLabelText));
                }
            }
        } string requestlabeltext;

        public string RequestMessageText
        {
            get { return requesmessagettext; }
            set
            {
                if (requesmessagettext != value)
                {
                    requesmessagettext = value;
                    OnPropertyChanged(nameof(RequestMessageText));
                }
            }
        } string requesmessagettext;

        public bool IsEditMode
        {
            get { return iseditmode; }
            set
            {
                if (iseditmode != value)
                {
                    iseditmode = value;
                    OnPropertyChanged("IsEditMode");
                }
            }
        } bool iseditmode;

        public ICommand ErrorOKCommand { protected set; get; }

        public ICommand OKCommand { protected set; get; }
        public ICommand CancelCommand { protected set; get; }
        public ICommand CancelChangesCommand { protected set; get; }

        public bool NotDisposed { get; set; } = true;

        public AsyncCancelationDispatcher ACD { get; set; }

        public bool NotNetOrConnection
        {
            get { return !IsNetAndConnection(); }
        }

        public BaseViewModel(INavigation navigation)
        {
            Navigation = navigation;
            ErrorOKCommand = new Command(ErrorOk);
            ACD = new AsyncCancelationDispatcher();
        }

        public virtual void ErrorOk()
        {
            State = ModelState.Normal;
        }


        public virtual void DisposeModel()
        {
            ACD.CancelAll();
            NotDisposed = false;
        }

        public static string ColorToHex(Color color)
        {
            int red = (int)(color.R * 255);
            int green = (int)(color.G * 255);
            int blue = (int)(color.B * 255);
            return String.Format("#{0:X2}{1:X2}{2:X2}", red, green, blue);
        }

        public static string ColorToHexAlfa(Color color)
        {
            int red = (int)(color.R * 255);
            int green = (int)(color.G * 255);
            int blue = (int)(color.B * 255);
            int alpha = (int)(color.A * 255);
            return String.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", red, green, blue, alpha);
        }

        private void ChangeState(ModelState state)
        {
            IsLoadingState = false;
            IsNormalState = false;
            IsErrorState = false;
            IsRequestState = false;
            IsNoDataState = false;

            switch (state)
            {
                case ModelState.Undefined:
                    break;
                case ModelState.Loading:
                    IsLoadingState = true;
                    break;
                case ModelState.Normal:
                    IsNormalState = true;
                    break;
                case ModelState.Error:
                    IsErrorState = true;
                    break;
                case ModelState.Request:
                    IsRequestState = true;
                    break;
                case ModelState.NoData:
                    IsNoDataState = true;
                    break;
                default:
                    throw new InvalidOperationException("Impossible value");
            }
        }

        public bool IsNetAndConnection()
        {
            bool rv = true;
            if (!Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
            {
                rv = false;
                State = ModelState.Error;
                ErrorText = "Internet not available";
            }
            else
            {
                if (!(Global.CurrentConnection is Helpers.NAV.Connection))
                {
                    rv = false;
                    State = ModelState.Error;
                    ErrorText = "Сonnection not created";
                }
            }
            return rv;
        }
    }
}
