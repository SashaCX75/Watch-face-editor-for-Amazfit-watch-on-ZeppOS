using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ControlLibrary
{
    public partial class UCtrl_HRV_Elm : ControlLibrary.UCtrl_AlarmClock_Elm
    {
        public UCtrl_HRV_Elm()
        {
            InitializeComponent();

            if (!AppUtils.IsLightTheme())
            {
                if (button_ElementName.Image != null) button_ElementName.Image = AppUtils.InvertColors(button_ElementName.Image);
            }
        }
    }
}
