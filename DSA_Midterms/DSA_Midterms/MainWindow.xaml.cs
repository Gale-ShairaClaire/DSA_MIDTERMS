using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private readonly List<Student> _studentList = new();
        public ObservableCollection<Student> Students { get; } = new();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            RefreshGrid();
        }

        private void RefreshGrid()
        {
            Students.Clear();
            foreach (var s in _studentList)
                Students.Add(s);
        }

        private Student? ReadInputs()
        {
            if (!int.TryParse(txtID.Text.Trim(), out int id))
            {
                MessageBox.Show("ID must be a whole number.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }
            if (!decimal.TryParse(txtSalary.Text.Trim(), out decimal salary))
            {
                MessageBox.Show("Salary must be a number.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }
            return new Student
            {
                ID = id,
                Name = txtName.Text.Trim(),
                Title = txtTitle.Text.Trim(),
                Salary = salary
            };
        }

        private void FillInputs(Student s)
        {
            txtID.Text = s.ID.ToString();
            txtName.Text = s.Name ?? "";
            txtTitle.Text = s.Title ?? "";
            txtSalary.Text = s.Salary.ToString("0.##");
        }

        private void ClearInputs()
        {
            txtID.Text = txtName.Text = txtTitle.Text = txtSalary.Text = "";
        }

        private void myDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (myDataGrid.SelectedItem is Student s)
                FillInputs(s);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var student = ReadInputs();
            if (student == null) return;

            if (_studentList.Exists(x => x.ID == student.ID))
            {
                MessageBox.Show($"A student with ID {student.ID} already exists.", "Duplicate ID",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _studentList.Add(student);
            RefreshGrid();
            ClearInputs();
            MessageBox.Show($"Student '{student.Name}' added successfully.", "Add", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (myDataGrid.SelectedItem is not Student selected)
            {
                MessageBox.Show("Select a row to remove.", "Remove", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _studentList.RemoveAll(x => x.ID == selected.ID);
            RefreshGrid();
            ClearInputs();
            MessageBox.Show($"Student '{selected.Name}' removed successfully.", "Remove", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (myDataGrid.SelectedItem is not Student selected)
            {
                MessageBox.Show("Select a row to update.", "Update", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var updated = ReadInputs();
            if (updated == null) return;

            var index = _studentList.FindIndex(x => x.ID == selected.ID);
            if (index < 0) return;

            _studentList[index] = updated;
            RefreshGrid();
            myDataGrid.SelectedItem = Students.Count > index ? Students[index] : null;
            MessageBox.Show($"Student '{updated.Name}' updated successfully.", "Update", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            _studentList.Clear();
            RefreshGrid();
            myDataGrid.UnselectAll();
            ClearInputs();
            MessageBox.Show("All records have been cleared.", "Clear", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public class Student
        {
            public int ID { get; set; }
            public string? Name { get; set; }
            public string? Title { get; set; }
            public decimal Salary { get; set; }
        }
    }
}