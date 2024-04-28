﻿using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Repository.Interfaces;
using BookingApp.Service;
using BookingApp.View;
using BookingApp.View.Guide;
using BookingApp.View.Owner;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.ViewModel.Guide
{
    class GuideMainViewModel : ViewModel
    {
        private static ObservableCollection<TourDTO> _toursTodayDTO { get; set; }
        private Boolean _doesActiveTourExist = false;
        private readonly TourService _tourService;
        private TourDTO _selectedTourDTO = null;
        private UserDTO _loggedInGuide;
        private RelayCommand _showActiveTourCommand;
        private RelayCommand _showAllToursCommand;
        private RelayCommand _showTourStatisticsCommand;
        private RelayCommand _logoutCommand;
       // public int RowCount => (ToursTodayDTO.Count + 1) / 2;
        public GuideMainViewModel(User guide)
        {
            _loggedInGuide = new UserDTO(guide);
            ITourRepository tourRepository = Injector.CreateInstance<ITourRepository>();
            IUserRepository userRepository = Injector.CreateInstance<IUserRepository>();
            ITouristRepository touristRepository = Injector.CreateInstance<ITouristRepository>();
            ITourReservationRepository tourReservationRepository = Injector.CreateInstance<ITourReservationRepository>();
            ITourReviewRepository tourReviewRepository = Injector.CreateInstance<ITourReviewRepository>();
            IVoucherRepository voucherRepository = Injector.CreateInstance<IVoucherRepository>();
            _tourService = new TourService(tourRepository, userRepository, touristRepository, tourReservationRepository, tourReviewRepository, voucherRepository);
            List<TourDTO> toursDTO = _tourService.GetTodayTours(guide).Select(tour => new TourDTO(tour)).ToList();
            _toursTodayDTO = new ObservableCollection<TourDTO>(toursDTO);
            _showActiveTourCommand = new RelayCommand(ShowActiveTour);
            _showAllToursCommand = new RelayCommand(ShowAllTours);
            _showTourStatisticsCommand = new RelayCommand(ShowTourStatistics);
            _logoutCommand = new RelayCommand(Logout);
            ActiveTourExists();
        }
        public ObservableCollection<TourDTO> ToursTodayDTO
        {
            get { return _toursTodayDTO; }
            set
            {
                _toursTodayDTO = value;
                OnPropertyChanged();
            }
        }
        private void ActiveTourExists()
        {
            foreach ( TourDTO tour in _toursTodayDTO )
            {
                if (tour.IsActive)
                {
                    _doesActiveTourExist = true;
                    break;
                }
                {
                    _doesActiveTourExist = false;
                }
            }
        }
        public TourDTO SelectedTourDTO
        {
            get { return _selectedTourDTO; }
            set
            {
                _selectedTourDTO = value;
                OnPropertyChanged();
                ShowActiveTour();
            }
        }
        public RelayCommand ShowActiveTourCommand
        {
            get { return _showActiveTourCommand; }
            set
            {
                _showActiveTourCommand = value;
                OnPropertyChanged();
            }
        }
        private void ShowActiveTour(/*object parameter*/)
        {
            //TourDTO selectedTour = parameter as TourDTO;
            if (_selectedTourDTO.CurrentKeyPoint != "finished")
            {
                if (_doesActiveTourExist == true && _selectedTourDTO.IsActive != true)
                {
                    MessageBox.Show("Another tour has already started", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                ActiveTourWindow tourDetailsWindow = new ActiveTourWindow(_selectedTourDTO, _doesActiveTourExist);
                tourDetailsWindow.ShowDialog();
                _tourService.Update(_selectedTourDTO.ToTourAllParam());
            }
            else
            {
                MessageBox.Show("This tour has already finished", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        public RelayCommand ShowAllToursCommand
        {
            get { return _showAllToursCommand; }
            set
            {
                _showAllToursCommand = value;
                OnPropertyChanged();
            }
        }
        private void ShowAllTours()
        {
            AllToursWindow allToursView = new AllToursWindow(_loggedInGuide);
            allToursView.Show();
        }
        public RelayCommand ShowTourStatisticsCommand
        {
            get { return _showTourStatisticsCommand; }
            set
            {
                _showTourStatisticsCommand = value;
                OnPropertyChanged();
            }
        }
        private void ShowTourStatistics()
        {
            TourStatisticsWindow tourStatistics = new TourStatisticsWindow();
            tourStatistics.Show();
        }
        public RelayCommand LogoutCommand
        {
            get { return _logoutCommand; }
            set
            {
                _logoutCommand = value;
                OnPropertyChanged();
            }
        }
        private void Logout()
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            GuideMainWindow.GetInstance().Close();
        }
    }
}
