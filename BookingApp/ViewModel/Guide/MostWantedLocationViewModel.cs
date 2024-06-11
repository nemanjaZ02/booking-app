﻿using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Model.Enums;
using BookingApp.Repository;
using BookingApp.Repository.Interfaces;
using BookingApp.Service;
using BookingApp.Validation;
using BookingApp.View;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static System.Net.Mime.MediaTypeNames;

namespace BookingApp.ViewModel.Guide
{
    public class MostWantedLocationViewModel : Validation.ValidationBase
    {
        private KeyPointService _keyPointService;
        private TourService _tourService;
        private OrdinaryTourRequestService _tourRequestService;
        private Languages _selectedLanguage;
        private RelayCommand _addImagesCommand;
        private RelayCommand _removeImageCommand;
        private List<BitmapImage> _imagePreviews;
        public ObservableCollection<BitmapImage> imagesCollection;
        private BitmapImage _selectedImage;
        private DateTime _selectedDate;
        private LocationDTO _mostWantedLocation;
        private RelayCommand _addDateCommand;
        private RelayCommand _submitCommand;
        private string _keyPointString;
        private TourDTO _tourDTO;
        private List<string> _images;
        private ObservableCollection<DateTime> _dates;
        private UserDTO _loggedGuide;
        public event EventHandler TourAdded;
        public MostWantedLocationViewModel(UserDTO guide)
        {
            _loggedGuide = guide;

            IMessageRepository messageRepository = Injector.CreateInstance<IMessageRepository>();
            ITourRepository tourRepository = Injector.CreateInstance<ITourRepository>();
            IUserRepository userRepository = Injector.CreateInstance<IUserRepository>();
            IKeyPointRepository keyPointsRepository = Injector.CreateInstance<IKeyPointRepository>();
            ITouristRepository touristRepository = Injector.CreateInstance<ITouristRepository>();
            ITourReservationRepository tourReservationRepository = Injector.CreateInstance<ITourReservationRepository>();
            ITourReviewRepository tourReviewRepository = Injector.CreateInstance<ITourReviewRepository>();
            IVoucherRepository voucherRepository = Injector.CreateInstance<IVoucherRepository>();
            IOrdinaryTourRequestRepository requestRepository = Injector.CreateInstance<IOrdinaryTourRequestRepository>();
            IAccommodationReservationChangeRequestRepository accommodationReservationChangeRequestRepository = Injector.CreateInstance<IAccommodationReservationChangeRequestRepository>();
            IAccommodationReservationRepository accommodationReservationRepository = Injector.CreateInstance<IAccommodationReservationRepository>();
            IAccommodationRepository accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();
            _tourRequestService = new OrdinaryTourRequestService(accommodationReservationChangeRequestRepository, accommodationReservationRepository, accommodationRepository, requestRepository, tourRepository, messageRepository, touristRepository, userRepository, tourReservationRepository, tourReviewRepository, voucherRepository);
            _keyPointService = new KeyPointService(keyPointsRepository);
            _tourService = new TourService(tourRepository, userRepository, touristRepository, tourReservationRepository, tourReviewRepository, voucherRepository);
            _imagePreviews = new List<BitmapImage>();
            imagesCollection = new ObservableCollection<BitmapImage>(_imagePreviews);
            _tourDTO = new TourDTO();
            _submitCommand = new RelayCommand(Submit);
            _dates = new ObservableCollection<DateTime>();
            _addDateCommand = new RelayCommand(AddDate);
            _submitCommand = new RelayCommand(Submit);
            _addImagesCommand = new RelayCommand(AddImages);
            _removeImageCommand = new RelayCommand(RemoveImage);
            _mostWantedLocation = new LocationDTO(_tourRequestService.GetMostWantedLocation());
            countries = new List<string> { "Austrija", "BiH", "Crna Gora", "Francuska", "Hrvatska", "Italija", "Makedonija", "Madjarska", "Njemacka", "Srbija", "Slovenija", "Spanija" };
        }
        public TourDTO TourDTO
        {
            get
            {
                return _tourDTO;
            }
            set
            {
                _tourDTO = value;
                OnPropertyChanged();
            }
        }
        private List<string> countries;
        public List<string> Countries
        {
            get { return countries; }
            set
            {
                countries = value;
                OnPropertyChanged();
            }
        }

        public void RemoveImage()
        {
            if (SelectedImage == null)
            {
                return;
            }

            int index = ImagePreviews.IndexOf(SelectedImage);
            if (index >= 0)
            {
                ImagePreviews.RemoveAt(index);
            }
            if (_images != null && _images.Count > index)
            {
                _images.RemoveAt(index);
            }
        }
        public RelayCommand RemoveImageCommand
        {
            get
            {
                return _removeImageCommand;
            }
            set
            {
                _removeImageCommand = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<BitmapImage> ImagePreviews
        {
            get
            {
                return imagesCollection;
            }
            set
            {
                imagesCollection = value;
                OnPropertyChanged();
            }
        }
        public BitmapImage SelectedImage
        {
            get
            {
                return _selectedImage;
            }
            set
            {
                _selectedImage = value;
                OnPropertyChanged();
            }

        }
        public string KeyPointsString
        {
            get
            {
                return _keyPointString;
            }
            set
            {
                _keyPointString = value;
                OnPropertyChanged();
            }
        }
        public LocationDTO MostWantedLocationDTO
        {
            get
            {
                return _mostWantedLocation;
            }
            set
            {
                _mostWantedLocation = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<DateTime> Dates
        {
            get { return _dates; }
            set
            {
                _dates = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand AddDateCommand
        {
            get { return _addDateCommand; }
            set
            {
                _addDateCommand = value;
                OnPropertyChanged();
            }
        }
        private void AddDate()
        {
            DateTime date = _selectedDate;
            Dates.Add(date);
        }
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                _selectedDate = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand AddImagesCommand
        {
            get { return _addImagesCommand; }
            set
            {
                _addImagesCommand = value;
                OnPropertyChanged();
            }
        }
        private void AddImages()
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;

            bool? response = openFileDialog.ShowDialog();

            if (response == true)
            {
                _images = openFileDialog.FileNames.ToList();
                var strings = openFileDialog.FileNames;
                _images = strings.ToList();

                for (int i = 0; i < _images.Count; i++)
                {
                    _images[i] = System.IO.Path.GetRelativePath(AppDomain.CurrentDomain.BaseDirectory, _images[i]).ToString();
                    strings[i] = Path.GetFullPath(_images[i]);
                    BitmapImage imageSource = new BitmapImage(new Uri(strings[i]));
                    ImagePreviews.Add(imageSource);
                }

            }
        }
        public RelayCommand SubmitCommand
        {
            get { return _submitCommand; }
            set
            {
                _submitCommand = value;
                OnPropertyChanged();
            }
        }
        private void Submit()
        {
            _tourDTO.Images = _images;
            _tourDTO.GuideId = _loggedGuide.Id;
            _tourDTO.LocationDTO = _mostWantedLocation;

            foreach (var date in _dates)
            {
                TourDTO tourDTO = new TourDTO(_tourDTO);
                tourDTO.BeginingTime = date;
                tourDTO.Images = _images;
                tourDTO.LocationDTO = _mostWantedLocation;

                Validate1();

                if (IsValid)
                {
                    string[] tourKeyPoints = _keyPointString.Split(',');
                    _tourDTO = new TourDTO(_tourService.Save(tourDTO.ToTourAllParam()));
                    SetKeyPoints(tourKeyPoints);
                }
            }

            }
        public IEnumerable<Languages> Languages
        {
            get
            {
                return Enum.GetValues(typeof(Languages)).Cast<Languages>();
            }
            set { }
        }
        public Languages SelectedLanguage
        {
            get { return _selectedLanguage; }
            set
            {
                _selectedLanguage = value;
                OnPropertyChanged();
            }
        }
        private void SetKeyPoints(string[] keyPoints)
        {
            int count = 0;
            foreach (var keyPoint in keyPoints)
            {
                if (count == 0)
                {
                    KeyPointDTO newPoint = new KeyPointDTO();
                    newPoint.Name = keyPoint;
                    newPoint.TourId = _tourDTO.Id;
                    newPoint.IsCurrent = false;
                    newPoint.HasPassed = false;
                    newPoint.Type = KeyPointsType.Begining;
                    _keyPointService.Save(newPoint.ToKeyPoint());
                }
                else if (count != keyPoints.Length - 1)
                {
                    KeyPointDTO newPoint = new KeyPointDTO();
                    newPoint.Name = keyPoint;
                    newPoint.TourId = _tourDTO.Id;
                    newPoint.IsCurrent = false;
                    newPoint.HasPassed = false;
                    newPoint.Type = KeyPointsType.Middle;
                    _keyPointService.Save(newPoint.ToKeyPoint());
                }
                else
                {
                    KeyPointDTO newPoint = new KeyPointDTO();
                    newPoint.Name = keyPoint;
                    newPoint.TourId = _tourDTO.Id;
                    newPoint.IsCurrent = false;
                    newPoint.HasPassed = false;
                    newPoint.Type = KeyPointsType.Ending;
                    _keyPointService.Save(newPoint.ToKeyPoint());
                }
                count++;
            }
        }
        protected override void ValidateSelf1()
        {
            if (_tourDTO.Language == Model.Enums.Languages.Afrikaans)
            {
                ValidationErrors["Language"] = "*Neophodno je unijeti jezik";
            }

            if (string.IsNullOrWhiteSpace(_tourDTO.Name))
            {
                ValidationErrors["Name"] = "*Neophodno je unijeti ime";
            }
            else if (_keyPointString.Split(',').Length < 2)
            {
                ValidationErrors["KeyPoints"] = "Neophodno je unijeti bar dvije kljucne tacke";
            }
            if (_dates.Count == 0)
            {
                ValidationErrors["Date"] = "*Neophodno je unijeti bar jedan datum";
            }
            if (!int.TryParse(_tourDTO.MaxTouristNumber.ToString(), out int capacity) || capacity <= 0)
            {
                ValidationErrors["MaxTouristNumber"] = "*Unesite validan broj turista";
            }
            if (string.IsNullOrWhiteSpace(_keyPointString))
            {
                ValidationErrors["KeyPoints"] = "*Neophodno je unijeti kljucne tacke";
            }
            if (!int.TryParse(_tourDTO.Duration.ToString(), out int duration) || duration <= 0)
            {
                ValidationErrors["Duration"] = "*Unesite validan broj za trajanje";
            }


            OnPropertyChanged(nameof(ValidationErrors));
        }

        protected override void ValidateSelf2()
        {
            throw new NotImplementedException();
        }
    }
}
