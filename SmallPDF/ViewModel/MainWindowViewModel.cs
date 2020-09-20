using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SmallPDF.Helpers;
using SmallPDF.Model;
using SmallPDF.Model.DTO;
using SmallPDF.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace SmallPDF.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {
        public MainWindowViewModel()
        {
            try
            {
                var tmpCurrencies = xmlService.GetElements<CurrencyCollection>(Settings.Default.AvailableCurrenciesFile);
                Currencies = new ObservableCollection<Currency>(tmpCurrencies.Currencies.OrderBy(_c => _c.Code));
            }
            catch (Exception)
            {

                throw;
            }
        }

        #region PROPERTIES
        //
        XMLDataService xmlService => new XMLDataService();

        public bool InputEnabled
        {
            get
            {
                return SelectedFromCurrency != null && SelectedToCurrency != null;
            }
        }

        private bool CanSave
        {
            get
            {
                return EditionMode &&
                    !String.IsNullOrEmpty(SelectedCurreny?.Code) &&
                    !String.IsNullOrEmpty(SelectedCurreny?.Name);
            }
        }

        private bool _editionMode;
        public bool EditionMode
        {
            get
            {
                return _editionMode;
            }
            set
            {
                if (_editionMode != value)
                {
                    _editionMode = value;
                    OnPropertyChanged("EditionMode");
                }
            }
        }

        private ObservableCollection<Currency> _currencies;
        public ObservableCollection<Currency> Currencies
        {
            get
            {
                if (_currencies == null)
                    return new ObservableCollection<Currency>();
                return _currencies;
            }
            set
            {
                if (_currencies != value)
                {
                    _currencies = value;
                    OnPropertyChanged("Currencies");
                }
            }
        }


        private Currency _selectedCurreny;
        public Currency SelectedCurreny
        {
            get { return _selectedCurreny; }
            set
            {
                if (_selectedCurreny != value)
                {
                    _selectedCurreny = value;
                    OnPropertyChanged("SelectedCurreny");
                }
            }
        }

        private Currency _selectedFromCurrency;
        public Currency SelectedFromCurrency
        {
            get { return _selectedFromCurrency; }
            set
            {
                if (_selectedFromCurrency != value)
                {
                    _selectedFromCurrency = value;
                    OnPropertyChanged("SelectedFromCurrency");
                    OnPropertyChanged("InputEnabled");
                }

                if (!String.IsNullOrEmpty(InputValue))
                {
                    PerformConversion();
                }
            }
        }

        private Currency _selectedToCurrency;
        public Currency SelectedToCurrency
        {
            get { return _selectedToCurrency; }
            set
            {
                if (_selectedToCurrency != value)
                {
                    _selectedToCurrency = value;
                    OnPropertyChanged("SelectedToCurrency");
                    OnPropertyChanged("InputEnabled");
                }

                if (!String.IsNullOrEmpty(InputValue))
                {
                    PerformConversion();
                }
            }
        }

        private double _inputValueExchange => double.Parse(InputValue);
        private string _inputValue;
        public string InputValue
        {
            get { return _inputValue; }
            set
            {
                double tmp;
                if (_inputValue != value && double.TryParse(value, out tmp))
                {
                    _inputValue = value;
                    PerformConversion();
                    OnPropertyChanged("InputValue");
                }
            }
        }

        private string _outputValue;
        public string OutputValue
        {
            get { return _outputValue; }
            set
            {
                if (_outputValue != value)
                {
                    _outputValue = value;
                    OnPropertyChanged("OutputValue");
                }
            }
        }

        private string _statusInfo;
        public string StatusInfo
        {
            get { return _statusInfo; }
            set
            {
                if (_statusInfo != value)
                {
                    _statusInfo = value;
                    OnPropertyChanged("StatusInfo");
                }
            }
        }


        public Action RequestClose { get; internal set; }
        #endregion

        #region METHODS
        public void PerformConversion()
        {
            try
            {
                var task = Task.Run(async () =>
                {
                    StatusInfo = Resources.msgConverting;
                    string from = SelectedFromCurrency.Code;
                    string to = SelectedToCurrency.Code;
                    var uriAPI = $"https://api.ratesapi.io/api/latest?base={from}&symbols={to}";
                    var apiResponse = await HTTPService.GET_CallAsync(uriAPI);
                    if (apiResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var rate = apiResponse.Content.ReadAsStringAsync().Result.GetValue(to);
                        var output = _inputValueExchange * (double)rate;
                        OutputValue = $"{_inputValueExchange} {from} <--> {Math.Round(output, 4)} {to}";
                        StatusInfo = Resources.msgConverted;
                    }
                    else
                    {
                        MessageBox.Show(Resources.msgFailedConversion, Resources.ttlMainWindow, MessageBoxButton.OK, MessageBoxImage.Error);
                        StatusInfo = Resources.msgFailedConversion;
                    }
                });
            }
            catch (Exception)
            {
                StatusInfo = Resources.msgError;
                throw;
            }
        }
        #endregion

        #region COMMANDS

        private RelayCommand _addCurrencyCommand;
        public RelayCommand AddCurrencyCommand
        {
            get
            {
                return _addCurrencyCommand
                  ?? (_addCurrencyCommand = new RelayCommand(
                      _ =>
                      {
                          EditionMode = true;
                          SelectedCurreny = new Currency();
                      }));
            }
        }

        private RelayCommand _editCurrencyCommand;
        public RelayCommand EditCurrencyCommand
        {
            get
            {
                return _editCurrencyCommand
                  ?? (_editCurrencyCommand = new RelayCommand(
                      _ =>
                      {
                          EditionMode = true;
                      }, _ => SelectedCurreny != null));
            }
        }

        private RelayCommand _deleteCurrencyCommand;
        public RelayCommand DeleteCurrencyCommand
        {
            get
            {
                return _deleteCurrencyCommand
                  ?? (_deleteCurrencyCommand = new RelayCommand(
                      _ =>
                      {
                          xmlService.RemoveElement<Currency>(Settings.Default.AvailableCurrenciesFile,
                                      Settings.Default.RootName,
                                      "Id", SelectedCurreny.Id.ToString());
                          Currencies.Remove(SelectedCurreny);
                          EditionMode = false;
                      }, _ => SelectedCurreny != null && SelectedCurreny.Id != 0));
            }
        }

        private RelayCommand _saveCurrencyCommand;
        public RelayCommand SaveCurrencyCommand
        {
            get
            {
                return _saveCurrencyCommand
                  ?? (_saveCurrencyCommand = new RelayCommand(
                      _ =>
                      {
                          if (SelectedCurreny.Id == 0)
                          {
                              SelectedCurreny.Id = Currencies.Max(_c => _c.Id) + 1;
                              Currencies.Add(SelectedCurreny);
                              Currencies = new ObservableCollection<Currency>(Currencies.OrderBy(_c => _c.Code));
                              xmlService.AddElement<Currency>(Settings.Default.AvailableCurrenciesFile,
                                  Settings.Default.CollectionName, Settings.Default.RootName,
                                  new Dictionary<string, object>()
                                  {
                                      {Settings.Default.IdProperty, SelectedCurreny.Id },
                                      {Settings.Default.CodeProperty, SelectedCurreny.Code },
                                      {Settings.Default.NameProperty, SelectedCurreny.Name },
                                      {Settings.Default.SymbolProperty, "" }
                                  });
                          }
                          else
                          {
                              if (MessageBox.Show(String.Format(Resources.msgCurrencyExists, SelectedCurreny.Code), Resources.ttlMainWindow, MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                              {
                                  xmlService.UpdateElement<Currency>(Settings.Default.AvailableCurrenciesFile,
                                      Settings.Default.RootName,
                                      "Id", SelectedCurreny.Id.ToString(),
                                      new Dictionary<string, object>()
                                      {
                                          {Settings.Default.IdProperty, SelectedCurreny.Id },
                                          {Settings.Default.CodeProperty, SelectedCurreny.Code },
                                          {Settings.Default.NameProperty, SelectedCurreny.Name },
                                          {Settings.Default.SymbolProperty, "" }
                                      });
                              }
                          }
                          EditionMode = false;
                      }, _ => (CanSave)));
            }
        }
        #endregion
    }
}
