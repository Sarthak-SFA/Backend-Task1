using System.Collections.ObjectModel;
using Microsoft.Extensions.Logging;
using Practice.Core.Dtos;
using Practice.Core.Requests;
using Practice.Persistence;

namespace Practice.Services.Services;

public sealed class StateService
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<StateService> _logger;

    public StateService(AppDbContext dbContext, ILogger<StateService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger;
    }

    public IEnumerable<StateDto> GetAll()
    {
        IList<StateDto> states = _dbContext.State
            .OrderBy(s => s.Id)
            .Select(s => new StateDto(
                s.Id,
                s.Name,
                s.Code
            ))
            .ToArray();

        return new ReadOnlyCollection<StateDto>(states);
    }

    public IEnumerable<StateDto>? UpdateStates(UpdateStatesRequest request)
    {
        try
        {
            if (request == null || request.States == null)
            {
                return null;
            }

            foreach (CreateStateRequest item in request.States)
            {
                if (string.IsNullOrWhiteSpace(item.Name))
                {
                    throw new ArgumentException("Name cannot be empty.");
                }

                if (string.IsNullOrWhiteSpace(item.Code))
                {
                    throw new ArgumentException("Code cannot be empty.");
                }
            }

            IList<State> dbStates = _dbContext.State.ToList();

            HashSet<int> incomingIds = request.States
                .Where(x => x.Id > 0)
                .Select(x => x.Id)
                .ToHashSet();

            List<State> statesToDelete = dbStates
                .Where(dbState => !incomingIds.Contains(dbState.Id))
                .ToList();

            if (statesToDelete.Count > 0)
            {
                _dbContext.State.RemoveRange(statesToDelete);
            }

            foreach (CreateStateRequest item in request.States)
            {
                if (item.Id == 0)
                {
                    State State = new State
                    {
                        Name = item.Name.Trim(),
                        Code = item.Code.Trim()
                    };

                    _dbContext.State.Add(State);
                }
                else
                {
                    State? existingState = dbStates.FirstOrDefault(x => x.Id == item.Id);

                    if (existingState == null)
                    {
                        continue;
                    }

                    existingState.Name = item.Name.Trim();
                    existingState.Code = item.Code.Trim();
                }
            }

            _dbContext.SaveChanges();

            IList<StateDto> states = _dbContext.State
                .OrderBy(s => s.Id)
                .Select(s => new StateDto(
                    s.Id,
                    s.Name,
                    s.Code
                ))
                .ToArray();

            return new ReadOnlyCollection<StateDto>(states);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while saving state changes.");
            return null;
        }
    }
}
