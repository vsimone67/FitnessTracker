﻿using FitnessTracker.Application.Model.Workout;
using FitnessTracker.Application.Workout.Command;
using FitnessTracker.Application.Workout.Queries;
using FitnessTracker.Application.Workout.Workout.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitnessTracker.Service.Controllers
{
    [Route("[controller]")]
    public class WorkoutController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<WorkoutController> _logger;

        public WorkoutController(IMediator mediator, ILogger<WorkoutController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [Route("GetWorkouts")]
        public async Task<IActionResult> GetWorkouts(string activeOnly)
        {
            _logger.LogInformation("Getting Workouts");

            var query = new GetAllWorkoutsQuery();

            if (!string.IsNullOrEmpty(activeOnly))  // true or null active only, false = get all
            {
                query.IsActive = bool.Parse(activeOnly);
            }

            List<WorkoutDTO> workout = await _mediator.Send<List<WorkoutDTO>>(query);
            return Ok(workout);
        }

        [HttpGet]
        [Route("GetWorkoutForDisplay/{id}")]
        public async Task<IActionResult> GetWorkoutForDisplay(int? id)
        {
            _logger.LogInformation("Getting Workouts For Display");

            WorkoutDisplayDTO workout = await _mediator.Send<WorkoutDisplayDTO>(new GetWorkoutForDisplayQuery() { Id = id.Value });
            return Ok(workout);
        }

        [HttpGet]
        [Route("GetBodyInfo")]
        public async Task<IActionResult> GetBodyInfo()
        {
            _logger.LogInformation("Getting Body Info");
            List<BodyInfoDTO> bodyInfo = await _mediator.Send<List<BodyInfoDTO>>(new GetBodyInfoQuery());
            return Ok(bodyInfo);
        }

        [HttpGet]
        [Route("GetSets")]
        public async Task<IActionResult> GetSets()
        {
            _logger.LogInformation("Getting Sets");
            var retSet = await _mediator.Send<List<SetNameDTO>>(new GetSetQuery());
            return Ok(retSet);
        }

        [HttpGet]
        [Route("GetExercises")]
        public async Task<IActionResult> GetExercises()
        {
            _logger.LogInformation("Getting Exercises");

            List<ExerciseNameDTO> exercise = await _mediator.Send<List<ExerciseNameDTO>>(new GetExercisesQuery());
            return Ok(exercise);
        }

        [HttpGet]
        [Route("GetReps")]
        public async Task<IActionResult> GetReps()
        {
            _logger.LogInformation("Getting Reps");
            List<RepsNameDTO> reps = await _mediator.Send<List<RepsNameDTO>>(new GetRepsQuery());
            return Ok(reps);
        }

        [HttpGet]
        [Route("GetLastSavedWorkout/{id}")]
        public async Task<IActionResult> GetLastSavedWorkout(int? id)
        {
            _logger.LogInformation($"Getting Last Saved Workout for {id}");

            List<DailyWorkoutDTO> savedWorkout = await _mediator.Send<List<DailyWorkoutDTO>>(new GetLastSavedWorkoutQuery() { Id = id.Value });
            return Ok(savedWorkout);
        }

        [HttpPost]
        [Route("SaveBodyInfo")]
        public async Task<IActionResult> SaveBodyInfo([FromBody] BodyInfoDTO item)
        {
            _logger.LogInformation("Saving Body Info");

            BodyInfoDTO savedBodyInfo = await _mediator.Send<BodyInfoDTO>(new SaveBodyInfoCommand() { BodyInfo = item });
            await _mediator.Send<Unit>(new SendBodyInfoToEventBusCommand() { BodyInfo = item });  // send to event bus

            return Ok(savedBodyInfo);
        }

        [HttpPost]
        [Route("SaveDailyWorkout")]
        public async Task<IActionResult> SaveDailyWorkout([FromBody] WorkoutDisplayDTO item)
        {
            _logger.LogInformation("Saving Daily Workout");
            DailyWorkoutDTO savedWorkout = await _mediator.Send<DailyWorkoutDTO>(new SaveDailyWorkoutCommand() { Workout = item });
            await _mediator.Send<Unit>(new SaveDailyWorkoutToEventBusCommand() { Workout = savedWorkout });  // send to event bus

            return Ok(savedWorkout);
        }

        [HttpPost]
        [Route("SaveWorkout")]
        public async Task<IActionResult> SaveWorkout([FromBody] WorkoutDTO item)
        {
            _logger.LogInformation("Saving Workout");
            WorkoutDTO savedWorkout = await _mediator.Send<WorkoutDTO>(new SaveWorkoutCommand() { Workout = item });
            await _mediator.Send<Unit>(new SaveWorkoutToEventBusCommand() { Workout = savedWorkout });  // send to event bus

            return Ok(savedWorkout);
        }

        [HttpPost]
        [Route("UpdateWorkout")]
        public async Task<IActionResult> UpdateWorkout([FromBody] WorkoutDTO item)
        {
            _logger.LogInformation("Updating Workout");

            WorkoutDTO savedWorkout = await _mediator.Send<WorkoutDTO>(new UpdateWorkoutCommand() { Workout = item });
            await _mediator.Send<Unit>(new UpdateWorkoutToEventBusCommand() { Workout = savedWorkout });  // send to event bus

            return Ok(savedWorkout);
        }
    }
}