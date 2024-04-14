﻿using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Serializer;
using BookingApp.View.Guide;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace BookingApp.Repository
{
    public class TourRepository
    {
        private const string FilePath = "../../../Resources/Data/tours.csv";
        private TourReservationRepository _tourReservationRepository {get; set;}

        private readonly Serializer<Tour> _serializer;

        private List<Tour> _tours;
     
        public TourRepository()
        {
            _serializer = new Serializer<Tour>();
            _tours = _serializer.FromCSV(FilePath);
        }

        public List<Tour> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public Tour GetById(int id) 
        {

            _tours = _serializer.FromCSV(FilePath);
            return _tours.FirstOrDefault(u => u.Id == id);

        }

        public Tour Save(Tour tour)
        {
            tour.Id = NextId();
            _tours = _serializer.FromCSV(FilePath);
            _tours.Add(tour);
            _serializer.ToCSV(FilePath, _tours);
            return tour;
        }

        public int NextId()
        {
            _tours = _serializer.FromCSV(FilePath);
            if (_tours.Count < 1)
            {
                return 1;
            }
            return _tours.Max(c => c.Id) + 1;
        }

        public void Delete(Tour tour)
        {
            _tours = _serializer.FromCSV(FilePath);
            Tour founded = _tours.Find(c => c.Id == tour.Id);
            _tours.Remove(founded);
            _serializer.ToCSV(FilePath, _tours);
        }

        public Tour Update(Tour tour)
        {
            _tours = _serializer.FromCSV(FilePath);
            Tour current = _tours.Find(c => c.Id == tour.Id);
            int index = _tours.IndexOf(current);
            _tours.Remove(current);
            _tours.Insert(index, tour);      
            _serializer.ToCSV(FilePath, _tours);
            return tour;
        }
        public List<Tour> GetToursWithSameLocation(Tour tour)
        {
            List<Tour> tours = new List<Tour>();
            foreach(Tour t in GetAll())
            {
                if(t.Place.Country==tour.Place.Country && t.Place.City==tour.Place.City && t.CurrentCapacity!=0)
                {
                    tours.Add(t);
                }
            }
            return tours;
        }
        public List<Tour> GetActiveTours()
        {
        _tourReservationRepository = new TourReservationRepository();
        List<Tour> activeTours = new List<Tour>();
            foreach (Tour t in GetAll())
            {
                foreach (TourReservation tr in _tourReservationRepository.GetAll())
                {
                    if (t.Id == tr.TourId && t.IsActive)
                    {
                        activeTours.Add(t);
                    }
                }
            }
            return activeTours;
        }

        public List<Tour> GetUnactiveTours()
        {
            _tourReservationRepository = new TourReservationRepository();
            List<Tour> unactiveTours = new List<Tour>();
            foreach (Tour t in GetAll())
            {
                foreach (TourReservation tr in _tourReservationRepository.GetAll())
                {
                    if (t.Id == tr.TourId && !t.IsActive)
                    {
                        unactiveTours.Add(t);
                    }
                }
            }
            return unactiveTours;
        }

        public List<Tour> GetFinishedTours()
        {
            _tourReservationRepository = new TourReservationRepository();
            List<Tour> finishedTours = new List<Tour>();
            foreach (Tour t in GetAll())
            {
                foreach (TourReservation tr in _tourReservationRepository.GetAll())
                {
                    if (t.Id == tr.TourId && t.CurrentKeyPoint.Equals("finished"))
                    {
                        finishedTours.Add(t);
                    }
                }
            }
            return finishedTours;
        }

    }
}
