using System.Collections.Generic;

namespace Unity.Game.SaveSystem
{
    public class GameSessionDTOMapper
    {
        public GameSessionHistoryRequest ToDTO(GameSession gameSession)
        {
            var submitHistoryDTOMapper = new SubmitHistoryDTOMapper();

            var gameSessionRequestHistoryDTO = new GameSessionHistoryRequest
            {
                MapId = gameSession.MapId,
                StartDatetime = gameSession.StartDatetime,
                EndDatetime = gameSession.EndDatetime ,
                SubmitHistories = new List<SubmitHistoryDTO>()
            };

            foreach (SubmitHistory submitHistory in gameSession.SubmitHistories)
            {
                SubmitHistoryDTO submitHistoryDTO = submitHistoryDTOMapper.ToDTO(submitHistory);

                gameSessionRequestHistoryDTO.SubmitHistories.Add(submitHistoryDTO);
            }

            return gameSessionRequestHistoryDTO;
        }
    }
}


