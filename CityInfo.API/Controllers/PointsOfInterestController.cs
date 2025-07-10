using CityInfo.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/cities/{cityId}/pointsofinterest")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<PointOfInterestDto>> GetPointsOfInterest(int cityId)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }
            return Ok(city.PointOfInterests);
        }

        [HttpGet("{pointofinterestid}", Name = "GetPointOfInterest")]
        public ActionResult<PointOfInterestDto> GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }
            var pointOfInterest = city.PointOfInterests.FirstOrDefault(p => p.Id == pointOfInterestId);
            if (pointOfInterest == null)
            {
                return NotFound();
            }
            return Ok(pointOfInterest);
        }

        public ActionResult<PointOfInterestForCreationDto> CreatePointOfInterest(
            int cityId,
            PointOfInterestForCreationDto pointOfInterestDto)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c=> c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }
            // TODO: Refactor this part when db added
            var maxPointOfInterestId = CitiesDataStore.Current.Cities.SelectMany(c => c.PointOfInterests).Max(p=> p.Id);
            var finalPointOfInterest = new PointOfInterestDto()
            {
                Id = maxPointOfInterestId + 1,
                Name = pointOfInterestDto.Name,
                Description = pointOfInterestDto.Description,
            };
            city.PointOfInterests.Add(finalPointOfInterest);
            return CreatedAtRoute("GetPointOfInterest", new
                {
                    cityId = cityId,
                    pointofinterestid = finalPointOfInterest.Id,
                },
                finalPointOfInterest);
        }

        [HttpPut("{pointofinterestid}")]
        public ActionResult UpdatePointOfInterest(int cityId, int pointofinterestid,
            PointOfInterestForUpdateDto poinOfInterest)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterestStore = city.PointOfInterests.FirstOrDefault(p => p.Id == pointofinterestid);
            if (pointOfInterestStore == null)
            {
                return NotFound();
            }
            pointOfInterestStore.Name = poinOfInterest.Name;
            pointOfInterestStore.Description = poinOfInterest.Description;
            return NoContent();

        }

        [HttpPatch("{pointofinterestid}")]
        public ActionResult PartiallyUpdatePointOfInterest(
            int cityId, int pointofinterestid,
            JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterestStore = city.PointOfInterests.FirstOrDefault(p => p.Id == pointofinterestid);
            if (pointOfInterestStore == null)
            {
                return NotFound();
            }
            var pointOfinterestToPatch = new PointOfInterestForUpdateDto()
            {
                Name = pointOfInterestStore.Name,
                Description = pointOfInterestStore.Description,
            };

            patchDocument.ApplyTo(pointOfinterestToPatch, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!TryValidateModel(pointOfinterestToPatch))
            {
                return BadRequest(ModelState);
            }
            pointOfInterestStore.Name = pointOfinterestToPatch.Name;
            pointOfInterestStore.Description = pointOfinterestToPatch.Description;
            return NoContent();

        }
    }
}
