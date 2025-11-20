using SmartKettle.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SmartKettle.ViewModels
{
    public class ContractViewModel : INotifyPropertyChanged
    {
        private KettleState state;
        private OperationContract selectedOperation;
        private bool preConditionsMet;
        private bool postConditionsMet;

        public ObservableCollection<OperationContract> Operations { get; set; }

        public OperationContract SelectedOperation
        {
            get => selectedOperation;
            set { selectedOperation = value; OnPropertyChanged(); }
        }

        public bool PreConditionsMet
        {
            get => preConditionsMet;
            set { preConditionsMet = value; OnPropertyChanged(); }
        }

        public bool PostConditionsMet
        {
            get => postConditionsMet;
            set { postConditionsMet = value; OnPropertyChanged(); }
        }

        public ContractViewModel(KettleState kettleState)
        {
            state = kettleState;
            InitializeOperations();
        }

        private void InitializeOperations()
        {
            Operations = new ObservableCollection<OperationContract>
            {
                new OperationContract
                {
                    Name = "Начать нагрев",
                    PreConditions = "Уровень воды > 0.5 л\nТемпература < 100°C\nПитание включено",
                    PostConditions = "Статус = 'Нагрев'\nТаймер запущен\nТемпература начала сохранена",
                    Effects = "Включает нагревательный элемент\nЗапускает таймер нагрева",
                    ExampleValid = "Вода: 1.0 л, Температура: 25°C, Питание: Вкл",
                    ExampleInvalid = "Вода: 0.3 л, Температура: 25°C, Питание: Вкл"
                },
                new OperationContract
                {
                    Name = "Остановить нагрев",
                    PreConditions = "Статус = 'Нагрев' или 'Поддержание'",
                    PostConditions = "Статус = 'Ожидание'\nНагрев выключен",
                    Effects = "Выключает нагревательный элемент",
                    ExampleValid = "Статус: Нагрев, Температура: 85°C",
                    ExampleInvalid = "Статус: Ожидание, Температура: 25°C"
                }
            };
        }

        public void CheckPreConditions()
        {
            if (SelectedOperation?.Name == "Начать нагрев")
            {
                PreConditionsMet = state.WaterLevel > 0.5 &&
                                 state.Temperature < 100 &&
                                 state.IsPowered;
            }
            else if (SelectedOperation?.Name == "Остановить нагрев")
            {
                PreConditionsMet = state.Status == "Нагрев" || state.Status == "Поддержание";
            }
        }

        public void ExecuteOperation()
        {
            if (SelectedOperation?.Name == "Начать нагрев" && PreConditionsMet)
            {
                state.Status = "Нагрев";
                state.IsHeating = true;
                PostConditionsMet = true;
            }
            else if (SelectedOperation?.Name == "Остановить нагрев" && PreConditionsMet)
            {
                state.Status = "Ожидание";
                state.IsHeating = false;
                PostConditionsMet = true;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}