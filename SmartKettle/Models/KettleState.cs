using System.ComponentModel;

namespace SmartKettle.Models
{
    public class KettleState : INotifyPropertyChanged
    {
        private double temperature;
        private double waterLevel;
        private bool isPowered;
        private bool isHeating;
        private string status;
        private double targetTemperature;
        private bool safetyLock;
        private bool overheat;

        public double Temperature
        {
            get => temperature;
            set { temperature = value; OnPropertyChanged(); }
        }

        public double WaterLevel
        {
            get => waterLevel;
            set { waterLevel = value; OnPropertyChanged(); }
        }

        public bool IsPowered
        {
            get => isPowered;
            set { isPowered = value; OnPropertyChanged(); }
        }

        public bool IsHeating
        {
            get => isHeating;
            set { isHeating = value; OnPropertyChanged(); }
        }

        public string Status
        {
            get => status;
            set { status = value; OnPropertyChanged(); }
        }

        public double TargetTemperature
        {
            get => targetTemperature;
            set { targetTemperature = value; OnPropertyChanged(); }
        }

        public bool SafetyLock
        {
            get => safetyLock;
            set { safetyLock = value; OnPropertyChanged(); }
        }

        public bool Overheat
        {
            get => overheat;
            set { overheat = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}