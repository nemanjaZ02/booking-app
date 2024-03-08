﻿using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for TourView.xaml
    /// </summary>
    public partial class TourView : Window
    {

        public static ObservableCollection<TourDTO> Tours { get; set; }
        private static TourDTO tour { get; set; }

        private readonly TourRepository _repository;

        public TourView()
        {
            InitializeComponent();
            DataContext = this;
            _repository = new TourRepository();
            Tours = new ObservableCollection<TourDTO>();
            tour = new TourDTO();
            Update();
        }
        private void Update()
        {
            Tours.Clear();
            foreach (Tour tour in _repository.GetAll()) Tours.Add(new TourDTO(tour));
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string searchInput = textboxSearch.Text.ToLower();
            string[] resultArray = searchInput.Split(',').Select(s => s.Trim()).ToArray();

            var filtered = Tours;

            if (resultArray.Length == 0 || string.IsNullOrWhiteSpace(searchInput))
            {
                dataGridTour.ItemsSource = Tours;
            }

            foreach (string input in resultArray)
            {
                string value = input;

                if (string.IsNullOrWhiteSpace(value))
                    continue; 

                filtered = FilterTours(filtered, value);
            }

            dataGridTour.ItemsSource = filtered;
        }

        private ObservableCollection<BookingApp.DTO.TourDTO> FilterTours(ObservableCollection<BookingApp.DTO.TourDTO> tours, string value)
        {
            var filtered = new ObservableCollection<BookingApp.DTO.TourDTO>();

            foreach (var tour in tours)
            {
                if (tour.Duration.ToString().Contains(value)
                    || tour.Language.ToLower().Contains(value)
                    || tour.BeginingTime.ToString().Contains(value)
                    || (tour.LocationDTO.City.ToLower() + ", " + tour.LocationDTO.Country.ToLower()).Contains(value)
                    || tour.MaxTouristNumber.ToString().Contains(value))
                {
                    filtered.Add(tour);
                }
            }

            return filtered;
        }

    }
}
