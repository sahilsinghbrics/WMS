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
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WarehouseControlSystem.Model;
using WarehouseControlSystem.ViewModel.Base;
using System.Threading.Tasks;

namespace WarehouseControlSystem.View.Pages.Base
{
    public class SchemeBasePlanPage : ContentPage
    {
        protected List<SchemeBaseView> Views { get; set; } = new List<SchemeBaseView>();
        protected List<SchemeBaseView> SelectedViews { get; set; } = new List<SchemeBaseView>();

        public PlanBaseViewModel BaseModel { get; set; }

        MovingActionTypeEnum MovingAction = MovingActionTypeEnum.None;

        readonly Easing easing1 = Easing.Linear;
        readonly Easing easingParcking = Easing.CubicInOut;

        double x;
        double y;

        double leftborder = double.MaxValue;
        double topborder = double.MaxValue;
        double rightborder = double.MinValue;
        double bottomborder = double.MinValue;

        double oldeTotalX;
        double oldeTotalY;

        protected TapGestureRecognizer TapGesture;
        protected PanGestureRecognizer PanGesture;

        public SchemeBasePlanPage(PlanBaseViewModel model)
        {
            BaseModel = model;
            BindingContext = BaseModel;

            TapGesture = new TapGestureRecognizer();
            PanGesture = new PanGestureRecognizer();
        }

        public void StackLayout_SizeChanged(object sender, EventArgs e)
        {
            StackLayout sl = (StackLayout)sender;
            BaseModel.SetScreenSizes(sl.Width, sl.Height, false);
        }

        public void Abslayout_SizeChanged(object sender, EventArgs e)
        {
            AbsoluteLayout al = (AbsoluteLayout)sender;
            BaseModel.SetScreenSizes(al.Width, al.Height, true);
        }

        protected void Reshape()
        {
            foreach (SchemeBaseView lv in Views)
            {
                AbsoluteLayout.SetLayoutBounds(lv, new Rectangle(lv.Model.ViewLeft, lv.Model.ViewTop, lv.Model.ViewWidth, lv.Model.ViewHeight));
            }
        }

        protected async void OnPaned(object sender, PanUpdatedEventArgs e)
        {
            if (CheckRules())
            {
                return;
            }

            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    {
                        SelectedViews = Views.FindAll(x => x.Model.IsSelected == true);
                        InitMovement();
                        x += oldeTotalX;
                        y += oldeTotalY;
                        break;
                    }
                case GestureStatus.Running:
                    {
                        double dx = x + e.TotalX;
                        double dy = y + e.TotalY;
                        oldeTotalX = e.TotalX;
                        oldeTotalY = e.TotalY;
                        await Move(dx, dy);
                        break;
                    }
                case GestureStatus.Completed:
                    {
                        x = 0;
                        y = 0;
                        oldeTotalX = 0;
                        oldeTotalY = 0;
                        await EndMove().ConfigureAwait(true);
                        BaseModel.SaveChangesAsync();
                        MovingAction = MovingActionTypeEnum.None;
                        break;
                    }
                case GestureStatus.Canceled:
                    {
                        MovingAction = MovingActionTypeEnum.None;
                        break;
                    }
                default:
                    throw new InvalidOperationException("Impossible value");
            }
        }

        private void InitMovement()
        {
            MovingAction = MovingActionTypeEnum.Pan;

            leftborder = double.MaxValue;
            topborder = double.MaxValue;
            rightborder = double.MinValue;
            bottomborder = double.MinValue;
            foreach (SchemeBaseView lv in SelectedViews)
            {
                leftborder = Math.Min(lv.X, leftborder);
                topborder = Math.Min(lv.Y, topborder);
                rightborder = Math.Max(lv.X + lv.Width, rightborder);
                bottomborder = Math.Max(lv.Y + lv.Height, bottomborder);
                lv.Opacity = 0.5;
                lv.Model.SavePrevSize(lv.Width, lv.Height);
            }
        }
        private double CorrectionDX(double dx)
        {
            double dxrv = dx;


            if (dx + leftborder < 0)
            {
                dxrv = -leftborder;
            }

            if (dx + rightborder > BaseModel.ScreenWidth)
            {
                dxrv = BaseModel.ScreenWidth - rightborder;
            }


            return dxrv;
        }

        private double CorrectionDY(double dy)
        {
            double dyrv = dy;

            if (dy + topborder < 0)
            {
                dyrv = -topborder;
            }

            if (dy + bottomborder > (BaseModel.ScreenHeight))
            {
                dyrv = BaseModel.ScreenHeight - bottomborder;
            }

            return dyrv;
        }
        private double ResizeCorrectionDX(double lvX, double lvwidth, double dx)
        {
            double dxrv = dx;


            if (dx + lvwidth < 0)
            {
                dxrv = -lvwidth+40;
            }

            if (dx + lvwidth + lvX > BaseModel.ScreenWidth)
            {
                dxrv = BaseModel.ScreenWidth - lvwidth - lvX;
            }


            return dxrv;
        }

        private double ResizeCorrectionDY(double lvY, double lbheight, double dy)
        {
            double dyrv = dy;

            if (dy + lbheight < 0)
            {
                dyrv = -lbheight + 40;
            }

            if (dy + lbheight + lvY > (BaseModel.ScreenHeight))
            {
                dyrv = BaseModel.ScreenHeight - lbheight - lvY;
            }

            return dyrv;
        }

        private async Task Move(double dx, double dy)
        {
            foreach (SchemeBaseView lv in SelectedViews)
            {
                if (lv.Model.EditMode == SchemeElementEditMode.Move)
                {
                    dx = CorrectionDX(dx);
                    dy = CorrectionDY(dy);
                    await lv.TranslateTo(dx, dy, 250, easing1);
                }
                if (lv.Model.EditMode == SchemeElementEditMode.Resize)
                {
                    dx = ResizeCorrectionDX(lv.X,lv.Model.PrevViewWidth, dx);
                    dy = ResizeCorrectionDY(lv.Y,lv.Model.PrevViewHeight, dy);
                    AbsoluteLayout.SetLayoutBounds(lv, new Rectangle(lv.X, lv.Y, lv.Model.PrevViewWidth + dx, lv.Model.PrevViewHeight + dy));
                }
            }
        }

        private async Task EndMove()
        {
            foreach (SchemeBaseView lv in SelectedViews)
            {
                if (lv.Model.EditMode == SchemeElementEditMode.Move)
                {
                    double newX = lv.X + lv.TranslationX;
                    double newY = lv.Y + lv.TranslationY;

                    lv.Model.Left = (int)Math.Round(newX / BaseModel.WidthStep);
                    lv.Model.Top = (int)Math.Round(newY / BaseModel.HeightStep);

                    double dX = lv.Model.Left * BaseModel.WidthStep - lv.X;
                    double dY = lv.Model.Top * BaseModel.HeightStep - lv.Y;

                    await lv.TranslateTo(dX, dY, 500, easingParcking);
                    AbsoluteLayout.SetLayoutBounds(lv, new Rectangle(lv.X + dX, lv.Y + dY, lv.Width, lv.Height));
                    lv.TranslationX = 0;
                    lv.TranslationY = 0;
                }

                if (lv.Model.EditMode == SchemeElementEditMode.Resize)
                {
                    lv.Model.Width = (int)Math.Round(lv.Width / BaseModel.WidthStep);
                    lv.Model.Height = (int)Math.Round(lv.Height / BaseModel.HeightStep);
                    double newWidth = lv.Model.Width * BaseModel.WidthStep;
                    double newheight = lv.Model.Height * BaseModel.HeightStep;
                    AbsoluteLayout.SetLayoutBounds(lv, new Rectangle(lv.X, lv.Y, newWidth, newheight));
                }
                lv.Opacity = 1;
            }
        }

        private bool CheckRules()
        {
            if (!BaseModel.IsEditMode)
            {
                return true;
            }

            if ((MovingAction != MovingActionTypeEnum.None) && (MovingAction != MovingActionTypeEnum.Pan))
            {
                return true;
            }

            //if (!Model.IsSelectedList)
            //{
            //    return true;
            //}

            return false;
        }
    }
}
