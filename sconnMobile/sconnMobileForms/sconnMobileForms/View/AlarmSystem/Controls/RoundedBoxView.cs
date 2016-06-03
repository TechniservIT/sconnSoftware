﻿using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace sconnMobileForms.View.AlarmSystem.Controls
{

    public class RoundedBox : BoxView
    {
        /// <summary>
        /// The corner radius property.
        /// </summary>
        public static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create("CornerRadius", typeof(double), typeof(RoundedBox), 0.0);

        /// <summary>
        /// Gets or sets the corner radius.
        /// </summary>
        public double CornerRadius
        {
            get { return (double)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
    }

}
