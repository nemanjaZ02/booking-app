﻿using BookingApp.DTO;
using BookingApp.ViewModel.Guest;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.View.Guest
{
    /// <summary>
    /// Interaction logic for FindAvailableDatesPage.xaml
    /// </summary>
    public partial class FindAvailableDatesPage : Page
    {
        FindAvailableDatesViewModel _findAvailableDatesViewModel;
        public static FindAvailableDatesPage Instance;
        public FindAvailableDatesPage(AccommodationDTO selectedAccommodationDTO, UserDTO loggedInGuest)
        {
            InitializeComponent();
            _findAvailableDatesViewModel = new FindAvailableDatesViewModel(selectedAccommodationDTO, loggedInGuest);
            DataContext = _findAvailableDatesViewModel;
            if(Instance == null )
            {
                Instance = this;
            }
        }
    }
}
