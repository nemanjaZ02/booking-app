using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookingApp.Service
{
    public class TourService
    {
        private TourRepository _tourRepository = new TourRepository();
        private TourReservationService _tourReservationService = new TourReservationService();
        public List<Tour> GetAll() 
        {
            return _tourRepository.GetAll();
        }

        public Tour GetById(int id)
        {
            return _tourRepository.GetById(id);
        }
        public Tour Save(Tour tour) 
        {
            return _tourRepository.Save(tour);
        }
        public void Delete(Tour tour)
        {
             _tourRepository.Delete(tour);
        }
        public Tour Update(Tour tour)
        {
            return _tourRepository.Update(tour);
        }
        public List<Tour> GetActiveTours()
        {
            List<Tour> activeTours = new List<Tour>();
                foreach (Tour tour in GetAll())
                {
                    foreach (TourReservation tourReservation in _tourReservationService.GetAll())
                    {
                        if (tour.Id == tourReservation.TourId && tour.IsActive)
                        {
                            activeTours.Add(tour);
                        }
                    }
                }
           return activeTours;
        }
        public List<Tour> GetAllActiveTours()
        {
            List<Tour> activeTours = new List<Tour>();
            foreach (Tour tour in GetAll())
            {
                if (tour.IsActive)
                {
                    activeTours.Add(tour);
                }
            }
            return activeTours;
        }
        public List<Tour> GetUnactiveTours()
        {
            List<Tour> unactiveTours = new List<Tour>();
            foreach (Tour tour in GetAll())
            {
                foreach (TourReservation tourReservation in _tourReservationService.GetAll())
                {
                    if (tour.Id == tourReservation.TourId && !tour.IsActive && !tour.CurrentKeyPoint.Equals("finished"))
                    {
                        unactiveTours.Add(tour);
                    }
                }
            }
            return unactiveTours;
        }
        public List<Tour> GetFinishedTours()
        {
            List<Tour> finishedTours = new List<Tour>();
            foreach (Tour tour in GetAll())
            {
                foreach (TourReservation tourReservation in _tourReservationService.GetAll())
                {
                    if (tour.Id == tourReservation.TourId && tour.CurrentKeyPoint.Equals("finished"))
                    {
                        finishedTours.Add(tour);
                    }
                }
            }
            return finishedTours.Distinct().ToList();
        }
        public List<Tour> GetAllFinishedTours()
        {
            List<Tour> finishedTours = new List<Tour>();
            foreach (Tour tour in GetAll())
            {
                    if (tour.CurrentKeyPoint.Equals("finished"))
                    {
                        finishedTours.Add(tour);
                    }

            }
            return finishedTours;
        }
        public Tour GetMostVisitedTour()
        {
            return _tourRepository.GetMostVisitedTour();
        }
        public Tour GetMostVisitedByYear(int year)
        {
            return _tourRepository.GetMostVisitedByYear(year);
        }
        public List<Tour> GetNotCancelled()
        {
            return _tourRepository.GetNotCancelled();
        }
        public List<Tour> GetTodayTours(User user)
        {
            List<Tour> toursToday = new List<Tour>();
            foreach (Tour tour in GetAll())
            {
                if (tour.BeginingTime.Date == DateTime.Today && tour.GuideId == user.Id)
                {
                    toursToday.Add(tour);
                }
            }
            return toursToday;
        }
    }
}
