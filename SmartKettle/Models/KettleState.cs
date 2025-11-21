using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SmartKettle.Models
{
    public class KettleState : INotifyPropertyChanged
    {
        private double _temperature = 25;
        private double _waterLevel = 1.0;
        private bool _isPowered = true;
        private bool _isHeating = false;
        private string _status = "Готов";
        private double _targetTemperature = 95;
        private bool _safetyLock = false;
        private bool _overheat = false;

        public double Temperature
        {
            get => _temperature;
            set
            {
                if (_temperature != value)
                {
                    _temperature = value;
                    OnPropertyChanged();

                    // Автоматически проверяем перегрев
                    if (_temperature > 100)
                    {
                        Overheat = true;
                        IsHeating = false;
                        Status = "Перегрев!";
                    }
                    else
                    {
                        Overheat = false;
                    }
                }
            }
        }

        public double WaterLevel
        {
            get => _waterLevel;
            set { _waterLevel = value; OnPropertyChanged(); }
        }

        public bool IsPowered
        {
            get => _isPowered;
            set { _isPowered = value; OnPropertyChanged(); }
        }

        public bool IsHeating
        {
            get => _isHeating;
            set
            {
                _isHeating = value;
                OnPropertyChanged();
            }
        }

        public string Status
        {
            get => _status;
            set { _status = value; OnPropertyChanged(); }
        }

        public double TargetTemperature
        {
            get => _targetTemperature;
            set { _targetTemperature = value; OnPropertyChanged(); }
        }

        public bool SafetyLock
        {
            get => _safetyLock;
            set { _safetyLock = value; OnPropertyChanged(); }
        }

        public bool Overheat
        {
            get => _overheat;
            set { _overheat = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}