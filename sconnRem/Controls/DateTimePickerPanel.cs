using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Timers;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Xml.Linq;
using System.Xml;
using sconnConnector;


namespace sconnRem.Controls
{
    public enum DateTimeType  { FromDate = 0x01, ToDate = 0x09 };

    class DateTimePickerPanel : StackPanel
    {
        private Grid DatePickerGrid;
        private DatePicker DateSelect;
        private ListBox HoursSelect;
        private ListBox MinutesSelect;
        private ListBox SecondsSelect;

        private void InitStaticFields()
        {     
            int columns = 3;
            int rows = 2;
            for (int i = 0; i < rows; i++)
            {
                DatePickerGrid.RowDefinitions.Add(new RowDefinition());
            }
            for (int i = 0; i < columns; i++)
            {
                DatePickerGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
        }

        private void InitComponents()
        {
            DatePickerGrid = new Grid();
            DateSelect = new DatePicker();
            HoursSelect = new ListBox();
            MinutesSelect = new ListBox();
            SecondsSelect = new ListBox();
            InitStaticFields();

            Grid.SetRow(DateSelect, 0);
            Grid.SetColumn(DateSelect, 0);
            DatePickerGrid.Children.Add(DateSelect);

            Grid.SetRow(HoursSelect, 1);
            Grid.SetColumn(HoursSelect, 0);
            for (int i = 0; i < 24; i++)
            {
                ListBoxItem item = new ListBoxItem();
                item.Content = i;
                HoursSelect.Items.Add(item);
            }
            HoursSelect.MaxHeight = 50;
            DatePickerGrid.Children.Add(HoursSelect);

            Grid.SetRow(MinutesSelect, 1);
            Grid.SetColumn(MinutesSelect, 1);
            for (int i = 0; i < 60; i++)
            {
                ListBoxItem item = new ListBoxItem();
                item.Content = i;
                MinutesSelect.Items.Add(item);
            }
            MinutesSelect.MaxHeight = 50;
            DatePickerGrid.Children.Add(MinutesSelect);

            Grid.SetRow(SecondsSelect, 1);
            Grid.SetColumn(SecondsSelect, 2);
            for (int i = 0; i < 60; i++)
            {
                ListBoxItem item = new ListBoxItem();
                item.Content = i;
                SecondsSelect.Items.Add(item);
            }
            SecondsSelect.MaxHeight = 50;
            DatePickerGrid.Children.Add(SecondsSelect);

        }

        public DateTimePickerPanel(DateTime date)
        {
            InitComponents();
            DateSelect.SelectedDate = date;
            HoursSelect.SelectedItem = HoursSelect.Items.GetItemAt(date.Hour);
            MinutesSelect.SelectedItem = MinutesSelect.Items.GetItemAt(date.Minute);
            SecondsSelect.SelectedItem = SecondsSelect.Items.GetItemAt(date.Second);
            this.Children.Add(DatePickerGrid);
            this.Visibility = Visibility.Visible;
            bool visisble = this.IsVisible;
        }

        public DateTimePickerPanel(byte[] schedule, byte type)
        {
            DateTime date = BinaryDateToDateTime(schedule, type);
            InitComponents();
            DateSelect.SelectedDate = date;
            HoursSelect.SelectedItem = HoursSelect.Items.GetItemAt(date.Hour);
            MinutesSelect.SelectedItem = MinutesSelect.Items.GetItemAt(date.Minute);
            SecondsSelect.SelectedItem = SecondsSelect.Items.GetItemAt(date.Second);
            this.Children.Add(DatePickerGrid);
            this.Visibility = Visibility.Visible;
            bool visisble = this.IsVisible;
        }


        public DateTime GetDate()
        {
            return (DateTime)DateSelect.SelectedDate;
        }

        public byte[] GetDateBinary()
        {
            DateTime seldate = (DateTime)DateSelect.SelectedDate;
            return DateTimeToBinaryDate(seldate); 
        }


        //start possition required for 'from - to' date memory mapping
        public void SetDateFromBinary(byte[] schedule, byte startpos)
        {
            try
            {
                DateTime dt = BinaryDateToDateTime(schedule, startpos);
                DateSelect.SelectedDate = dt;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private byte[] DateTimeToBinaryDate(DateTime date)
        {
            byte[] raw = new byte[ipcDefines.SCHED_TIMEDATE_SIZE];   //return 8byte time and date
            try
            {
                raw[ipcDefines.SCHED_TIME_HR_POS] = Byte.Parse( ((ListBoxItem)HoursSelect.SelectedItem).Content.ToString() );
                raw[ipcDefines.SCHED_TIME_MIN_POS] = Byte.Parse( ((ListBoxItem)MinutesSelect.SelectedItem).Content.ToString());
                raw[ipcDefines.SCHED_TIME_SEC_POS] = Byte.Parse( ((ListBoxItem)SecondsSelect.SelectedItem).Content.ToString());

                raw[ipcDefines.SCHED_DATE_YR_POS + 4] = (byte) (date.Year%100); //return 2 digit year BCD
                raw[ipcDefines.SCHED_DATE_MON_POS + 4] = (byte)date.Month;
                raw[ipcDefines.SCHED_DATE_MDAY_POS + 4] = (byte)date.Day;
            }
            catch (Exception)
            {
                
                throw;
            }
            return raw;
        }

        private DateTime BinaryDateToDateTime(byte[] schedule, byte startpos)  //startposition in sequence depends on 'from' and 'to' date
        {
            DateTime dt = new DateTime();
            try
            {
                // time is MSb: hour, min, sec, rsvd. date is MSb: year, mon, mday, wday.
                int yearstoadd = (int)schedule[startpos + ipcDefines.SCHED_DATE_YR_POS] + 2000;
                dt = dt.AddYears(yearstoadd); //add 2000 years for current epoch, year is 1-byte encoded.
                dt = dt.AddMonths((int)schedule[startpos + ipcDefines.SCHED_DATE_MON_POS]);
                dt = dt.AddDays((double)schedule[startpos + ipcDefines.SCHED_DATE_MDAY_POS]);
                dt = dt.AddHours( (double)schedule[startpos + ipcDefines.SCHED_TIME_HR_POS] );
                dt = dt.AddSeconds( (double) schedule[startpos+ipcDefines.SCHED_TIME_SEC_POS]);
                
            }
            catch (Exception e)
            {
                throw;
            }
            return dt;
        }

    }

}
