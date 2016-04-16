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
        private Grid _datePickerGrid;
        private DatePicker _dateSelect;
        private ListBox _hoursSelect;
        private ListBox _minutesSelect;
        private ListBox _secondsSelect;

        private void InitStaticFields()
        {     
            int columns = 3;
            int rows = 2;
            for (int i = 0; i < rows; i++)
            {
                _datePickerGrid.RowDefinitions.Add(new RowDefinition());
            }
            for (int i = 0; i < columns; i++)
            {
                _datePickerGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
        }

        private void InitComponents()
        {
            _datePickerGrid = new Grid();
            _dateSelect = new DatePicker();
            _hoursSelect = new ListBox();
            _minutesSelect = new ListBox();
            _secondsSelect = new ListBox();
            InitStaticFields();

            Grid.SetRow(_dateSelect, 0);
            Grid.SetColumn(_dateSelect, 0);
            _datePickerGrid.Children.Add(_dateSelect);

            Grid.SetRow(_hoursSelect, 1);
            Grid.SetColumn(_hoursSelect, 0);
            for (int i = 0; i < 24; i++)
            {
                ListBoxItem item = new ListBoxItem();
                item.Content = i;
                _hoursSelect.Items.Add(item);
            }
            _hoursSelect.MaxHeight = 50;
            _datePickerGrid.Children.Add(_hoursSelect);

            Grid.SetRow(_minutesSelect, 1);
            Grid.SetColumn(_minutesSelect, 1);
            for (int i = 0; i < 60; i++)
            {
                ListBoxItem item = new ListBoxItem();
                item.Content = i;
                _minutesSelect.Items.Add(item);
            }
            _minutesSelect.MaxHeight = 50;
            _datePickerGrid.Children.Add(_minutesSelect);

            Grid.SetRow(_secondsSelect, 1);
            Grid.SetColumn(_secondsSelect, 2);
            for (int i = 0; i < 60; i++)
            {
                ListBoxItem item = new ListBoxItem();
                item.Content = i;
                _secondsSelect.Items.Add(item);
            }
            _secondsSelect.MaxHeight = 50;
            _datePickerGrid.Children.Add(_secondsSelect);

        }

        public DateTimePickerPanel(DateTime date)
        {
            InitComponents();
            _dateSelect.SelectedDate = date;
            _hoursSelect.SelectedItem = _hoursSelect.Items.GetItemAt(date.Hour);
            _minutesSelect.SelectedItem = _minutesSelect.Items.GetItemAt(date.Minute);
            _secondsSelect.SelectedItem = _secondsSelect.Items.GetItemAt(date.Second);
            this.Children.Add(_datePickerGrid);
            this.Visibility = Visibility.Visible;
            bool visisble = this.IsVisible;
        }

        public DateTimePickerPanel(byte[] schedule, byte type)
        {
            DateTime date = BinaryDateToDateTime(schedule, type);
            InitComponents();
            _dateSelect.SelectedDate = date;
            _hoursSelect.SelectedItem = _hoursSelect.Items.GetItemAt(date.Hour);
            _minutesSelect.SelectedItem = _minutesSelect.Items.GetItemAt(date.Minute);
            _secondsSelect.SelectedItem = _secondsSelect.Items.GetItemAt(date.Second);
            this.Children.Add(_datePickerGrid);
            this.Visibility = Visibility.Visible;
            bool visisble = this.IsVisible;
        }


        public DateTime GetDate()
        {
            return (DateTime)_dateSelect.SelectedDate;
        }

        public byte[] GetDateBinary()
        {
            DateTime seldate = (DateTime)_dateSelect.SelectedDate;
            return DateTimeToBinaryDate(seldate); 
        }


        //start possition required for 'from - to' date memory mapping
        public void SetDateFromBinary(byte[] schedule, byte startpos)
        {
            try
            {
                DateTime dt = BinaryDateToDateTime(schedule, startpos);
                _dateSelect.SelectedDate = dt;
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
                raw[ipcDefines.SCHED_TIME_HR_POS] = Byte.Parse( ((ListBoxItem)_hoursSelect.SelectedItem).Content.ToString() );
                raw[ipcDefines.SCHED_TIME_MIN_POS] = Byte.Parse( ((ListBoxItem)_minutesSelect.SelectedItem).Content.ToString());
                raw[ipcDefines.SCHED_TIME_SEC_POS] = Byte.Parse( ((ListBoxItem)_secondsSelect.SelectedItem).Content.ToString());

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
