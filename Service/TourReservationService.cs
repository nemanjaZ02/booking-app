﻿using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Service
{
    public class TourReservationService
    {
        private TourReservationRepository _tourReservationRepository = new TourReservationRepository();

        private TourReviewService _tourReviewService = new TourReviewService();
        private TouristService _touristService = new TouristService();
        private UserService _userService = new UserService();


        public List<TourReservation> GetAll()
        {
            return _tourReservationRepository.GetAll();
        }

        public TourReservation Save(TourReservation tourReservation)
        {
            return _tourReservationRepository.Save(tourReservation);
        }

        public void Delete(TourReservation tourReservation)
        {
            _tourReservationRepository.Delete(tourReservation);
        }

        public TourReservation Update(TourReservation tourReservation) 
        {
            return _tourReservationRepository.Update(tourReservation);
        }

        public TourReservation GetById(int id)
        { 
            return _tourReservationRepository.GetById(id);
        }

        public List<Tourist> GetJoinedTourists(Tour tour) 
        {
            List<Tourist> turisti = new List<Tourist>();
            foreach (TourReservation tourReservation in GetAll())
            {
                if (tour.Id == tourReservation.TourId)
                {
                    foreach (Model.Tourist tourist in tourReservation.Tourists)
                    {
                        if (tourist.JoiningKeyPoint != "")
                        {
                            AddReview(tourReservation, tourist);
                            _touristService.Save(tourist);
                            turisti.Add(tourist);
                        }
                    }
                }

            }
            return turisti;
        }

        private void AddReview(TourReservation tourReservation, Model.Tourist tourist)
        {
            foreach (TourReview tourReview in _tourReviewService.GetAll())
            {
                if (tourReview.TouristId == tourReservation.UserId && tourist.Name == _userService.GetById(tourReview.TouristId).Username && tourReservation.TourId == tourReview.TourId)
                {
                    tourist.Review = tourReview;
                }
            }
        }

    }
}
