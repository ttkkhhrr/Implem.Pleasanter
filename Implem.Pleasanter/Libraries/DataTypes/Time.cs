﻿using Implem.Libraries.Utilities;
using Implem.Pleasanter.Interfaces;
using Implem.Pleasanter.Libraries.Converts;
using Implem.Pleasanter.Libraries.Responses;
using Implem.Pleasanter.Libraries.Settings;
using Implem.Pleasanter.Libraries.Utilities;
using Implem.Pleasanter.Libraries.ViewParts;
using System;
using System.Data;
using System.Runtime.Serialization;
namespace Implem.Pleasanter.Libraries.DataTypes
{
    public class Time : IConvertable
    {
        public DateTime Value;
        public DateTime DisplayValue;

        public Time()
        {
        }

        public Time(DataRow dataRow, string name)
        {
            Value = dataRow.DateTime(name);
            DisplayValue = Value.ToLocal();
        }

        public Time(DateTime value, bool byForm = false)
        {
            Value = byForm
                ? value.ToUniversal()
                : value;
            DisplayValue = value;
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext streamingContext)
        {
            DisplayValue = Value.ToUniversal();
        }

        public virtual string ToView(Column column)
        {
            return Value.ToText(column);
        }

        public virtual string ToControl(Column column)
        {
            return Value.NotZero()
                ? DisplayValue.ToControl(column)
                : string.Empty;
        }

        public virtual string ToResponse()
        {
            return Value.NotZero()
                ? DisplayValue.ToString()
                : string.Empty;
        }

        public override string ToString()
        {
            return Value.NotZero() 
                ? Value.ToString() 
                : string.Empty;
        }

        public virtual string ToViewText(string format = "")
        {
            return Value.NotZero() 
                ? DisplayValue.ToString(format)
                : string.Empty;
        }

        public bool DifferentDate()
        {
            return 
                DisplayValue.ToShortDateString() !=
                DateTime.Now.ToLocal(Displays.YmdFormat());
        }

        public virtual HtmlBuilder Td(HtmlBuilder hb, Column column)
        {
            return hb.Td(action: () => hb
                .P(css: "time", action: () => hb
                    .Text(DisplayValue.ToText(column))));
        }

        public string ToExport(Column column)
        {
            return ToViewText();
        }
    }
}