using System.Windows;
using System.Windows.Controls;
using SmartKettle.ViewModels;

namespace SmartKettle
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void StartHeating_Click(object sender, RoutedEventArgs e)
        {
            var vm = (MainViewModel)DataContext;

            // Проверяем предусловия с учетом новых свойств безопасности
            if (vm.KettleState.WaterLevel > 0.5 &&
                vm.KettleState.Temperature < 100 &&
                vm.KettleState.IsPowered &&
                !vm.KettleState.SafetyLock &&
                !vm.KettleState.Overheat)
            {
                vm.KettleState.IsHeating = true;
                vm.KettleState.Status = "Нагрев";
                MessageBox.Show("Нагрев начат! Температура будет увеличиваться при нажатии 'Шаг цикла'.",
                    "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                string message = "Не выполнены предусловия для начала нагрева!\n";
                if (vm.KettleState.WaterLevel <= 0.5) message += "- Уровень воды должен быть > 0.5 л\n";
                if (vm.KettleState.Temperature >= 100) message += "- Температура должна быть < 100°C\n";
                if (!vm.KettleState.IsPowered) message += "- Питание должно быть включено\n";
                if (vm.KettleState.SafetyLock) message += "- Снят предохранитель безопасности\n";
                if (vm.KettleState.Overheat) message += "- Обнаружен перегрев\n";

                MessageBox.Show(message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void StopHeating_Click(object sender, RoutedEventArgs e)
        {
            var vm = (MainViewModel)DataContext;
            vm.KettleState.IsHeating = false;
            vm.KettleState.Status = "Ожидание";
            MessageBox.Show("Нагрев остановлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void CheckPreConditions_Click(object sender, RoutedEventArgs e)
        {
            var vm = (MainViewModel)DataContext;
            if (vm.ContractVM.SelectedOperation != null)
            {
                vm.ContractVM.CheckPreConditions();
                string result = vm.ContractVM.PreConditionsMet ? "выполнены" : "не выполнены";
                MessageBox.Show($"Предусловия {result}!", "Проверка", MessageBoxButton.OK,
                    vm.ContractVM.PreConditionsMet ? MessageBoxImage.Information : MessageBoxImage.Warning);
            }
            else
            {
                MessageBox.Show("Выберите операцию из списка!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ExecuteOperation_Click(object sender, RoutedEventArgs e)
        {
            var vm = (MainViewModel)DataContext;
            if (vm.ContractVM.SelectedOperation != null)
            {
                vm.ContractVM.ExecuteOperation();
                if (vm.ContractVM.PostConditionsMet)
                {
                    MessageBox.Show("Операция выполнена успешно!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Не удалось выполнить операцию! Проверьте предусловия.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Выберите операцию из списка!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ShowContract_Click(object sender, RoutedEventArgs e)
        {
            var vm = (MainViewModel)DataContext;
            if (vm.ContractVM.SelectedOperation != null)
            {
                MessageBox.Show(
                    $"Контракт: {vm.ContractVM.SelectedOperation.Name}\n\n" +
                    $"Предусловия:\n{vm.ContractVM.SelectedOperation.PreConditions}\n\n" +
                    $"Постусловия:\n{vm.ContractVM.SelectedOperation.PostConditions}",
                    "Контракт операции",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Выберите операцию из списка!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void OnWPPresetChanged(object sender, SelectionChangedEventArgs e)
        {
            if (WPPresetsComboBox.SelectedItem != null)
            {
                var vm = (MainViewModel)DataContext;
                vm.WPVM.LoadPreset(WPPresetsComboBox.SelectedItem.ToString());
<<<<<<< HEAD

                // Очищаем предыдущие результаты
                vm.WPVM.StepByStepTrace = "";
                vm.WPVM.FinalWP = "";
                vm.WPVM.HoareTriple = "";

=======
>>>>>>> 57c2e45cdd9180e2f5d1e7b4fc59ec2e3d9f5877
                MessageBox.Show($"Загружен пресет: {WPPresetsComboBox.SelectedItem}", "WP-калькулятор",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void CalculateWP_Click(object sender, RoutedEventArgs e)
        {
            var vm = (MainViewModel)DataContext;
<<<<<<< HEAD

            if (string.IsNullOrEmpty(vm.WPVM.PostCondition) || string.IsNullOrEmpty(vm.WPVM.Code))
            {
                MessageBox.Show("Сначала загрузите пресет или введите код и постусловие!", "WP-калькулятор",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            vm.WPVM.CalculateWP();

            string presetName = WPPresetsComboBox.SelectedItem?.ToString() ?? "текущий";
            MessageBox.Show($"WP для пресета '{presetName}' рассчитан успешно!", "WP-калькулятор",
                MessageBoxButton.OK, MessageBoxImage.Information);
=======
            vm.WPVM.CalculateWP();
            MessageBox.Show("WP рассчитан успешно!", "WP-калькулятор", MessageBoxButton.OK, MessageBoxImage.Information);
>>>>>>> 57c2e45cdd9180e2f5d1e7b4fc59ec2e3d9f5877
        }

        private void ExecuteStep_Click(object sender, RoutedEventArgs e)
        {
            var vm = (MainViewModel)DataContext;

            if (!vm.KettleState.IsHeating)
            {
                MessageBox.Show("Сначала включите нагрев кнопкой 'Начать нагрев'!", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (vm.KettleState.Temperature >= vm.KettleState.TargetTemperature)
            {
                MessageBox.Show("Целевая температура уже достигнута!", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            vm.InvariantVM.ExecuteStep();
            MessageBox.Show($"Шаг цикла выполнен!\nТекущая температура: {vm.KettleState.Temperature}°C\nЦелевая температура: {vm.KettleState.TargetTemperature}°C",
                "Инварианты", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void CalculateTruthTable_Click(object sender, RoutedEventArgs e)
        {
            var vm = (MainViewModel)DataContext;
            vm.BooleanVM.CalculateTruthTable();
            MessageBox.Show("Таблица истинности построена!", "Булевы функции",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void GenerateDNFKNF_Click(object sender, RoutedEventArgs e)
        {
            var vm = (MainViewModel)DataContext;
            vm.BooleanVM.GenerateDNFKNF();
            MessageBox.Show("DNF и KNF сгенерированы!", "Булевы функции",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void CheckEquivalence_Click(object sender, RoutedEventArgs e)
        {
            var vm = (MainViewModel)DataContext;
            vm.BooleanVM.CheckEquivalence();
            MessageBox.Show("Проверка эквивалентности выполнена!", "Булевы функции",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}