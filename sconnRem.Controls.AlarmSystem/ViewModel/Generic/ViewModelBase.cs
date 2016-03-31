using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace sconnRem.ViewModel.Generic
{
    
    public interface IGridNavigatedView : IGridView
    {

    }

    public interface IGridView
    {
        void Show();
        void Close();
        void Back();
        object DataContext { get; set; }
    }

    public class ViewModelBase<TViewType> : INotifyPropertyChanged  where TViewType : IGridView
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly TViewType _view;
        public TViewType View
        {
            get
            {
                return this._view;
            }
        }
        public ViewModelBase(TViewType view)
        {
            this._view = view;
           this.View.DataContext = this;
        }
        public void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    //public class RelayCommand : ICommand
    //{
    //    public event EventHandler CanExecuteChanged
    //    {
    //        add { CommandManager.RequerySuggested += value; }
    //        remove { CommandManager.RequerySuggested -= value; }
    //    }
    //    private Action methodToExecute;
    //    private Func<bool> canExecuteEvaluator;
    //    public RelayCommand(Action methodToExecute, Func<bool> canExecuteEvaluator)
    //    {
    //        this.methodToExecute = methodToExecute;
    //        this.canExecuteEvaluator = canExecuteEvaluator;
    //    }
    //    public RelayCommand(Action methodToExecute)
    //        : this(methodToExecute, null)
    //    {
    //    }
    //    public bool CanExecute(object parameter)
    //    {
    //        if (this.canExecuteEvaluator == null)
    //        {
    //            return true;
    //        }
    //        else
    //        {
    //            bool result = this.canExecuteEvaluator.Invoke();
    //            return result;
    //        }
    //    }
    //    public void Execute(object parameter)
    //    {
    //        this.methodToExecute.Invoke();
    //    }
    //}





    public class RelayCommand : ICommand
    {
        #region Fields

        readonly Action<object> _execute;
        readonly Predicate<object> _canExecute;

        #endregion // Fields

        #region Constructors

        /// <summary>
        /// Creates a new command that can always execute.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        public RelayCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion // Constructors

        #region ICommand Members

        [DebuggerStepThrough]
        public bool CanExecute(object parameters)
        {
            return _canExecute == null ? true : _canExecute(parameters);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameters)
        {
            _execute(parameters);
        }

        #endregion // ICommand Members
    }


    public interface IPageViewModel
    {
        string Name { get; }
    }



    public abstract class ObservableObject : INotifyPropertyChanged
    {
        #region Debugging Aides

        /// <summary>
        /// Warns the developer if this object does not have
        /// a public property with the specified name. This 
        /// method does not exist in a Release build.
        /// </summary>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public virtual void VerifyPropertyName(string propertyName)
        {
            // Verify that the property name matches a real,  
            // public, instance property on this object.
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                string msg = "Invalid property name: " + propertyName;

                if (this.ThrowOnInvalidPropertyName)
                    throw new Exception(msg);
                else
                    Debug.Fail(msg);
            }
        }

        /// <summary>
        /// Returns whether an exception is thrown, or if a Debug.Fail() is used
        /// when an invalid property name is passed to the VerifyPropertyName method.
        /// The default value is false, but subclasses used by unit tests might 
        /// override this property's getter to return true.
        /// </summary>
        protected virtual bool ThrowOnInvalidPropertyName { get; private set; }

        #endregion // Debugging Aides

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Raises the PropertyChange event for the property specified
        /// </summary>
        /// <param name="propertyName">Property name to update. Is case-sensitive.</param>
        public virtual void RaisePropertyChanged(string propertyName)
        {
            this.VerifyPropertyName(propertyName);
            OnPropertyChanged(propertyName);
        }

        /// <summary>
        /// Raised when a property on this object has a new value.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The property that has a new value.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.VerifyPropertyName(propertyName);

            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        #endregion // INotifyPropertyChanged Members
    }



}
