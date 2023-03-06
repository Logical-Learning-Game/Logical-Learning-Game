using System.Collections.Generic;

namespace Unity.Game.SaveSystem
{
    public class SubmitHistoryDTOMapper
    {
        public SubmitHistoryDTO ToDTO(SubmitHistory submitHistory)
        {
            var submitHistoryDTO = new SubmitHistoryDTO
            {
                IsFinited = submitHistory.IsFinited,
                IsCompleted = submitHistory.IsCompleted,
                CommandMedal = submitHistory.CommandMedal,
                ActionMedal = submitHistory.ActionMedal,
                SubmitDatetime = submitHistory.SubmitDatetime,
                StateValue = new StateValueDTO
                {
                    CommandCount = submitHistory.StateValue.CommandCount,
                    ForwardCommandCount = submitHistory.StateValue.ForwardCommandCount,
                    RightCommandCount = submitHistory.StateValue.RightCommandCount,
                    BackCommandCount = submitHistory.StateValue.BackCommandCount,
                    LeftCommandCount = submitHistory.StateValue.LeftCommandCount,
                    ConditionCommandCount = submitHistory.StateValue.ConditionCommandCount,
                    ActionCount = submitHistory.StateValue.ActionCount,
                    ForwardActionCount = submitHistory.StateValue.ForwardActionCount,
                    RightActionCount = submitHistory.StateValue.RightActionCount,
                    BackActionCount = submitHistory.StateValue.BackActionCount,
                    LeftActionCount = submitHistory.StateValue.LeftActionCount,
                    ConditionActionCount = submitHistory.StateValue.ConditionActionCount,
                },
                RuleHistories = new List<RuleHistoryRequestDTO>(),
                CommandNodes = new List<CommandNodeDTO>(),
                CommandEdges = new List<CommandEdgeDTO>(),
            };

            // mock map rule id!!!
            //long mockMapRuleId = 1;
            foreach (RuleHistory ruleHistory in submitHistory.RuleHistories)
            {
                var ruleHistoryDTO = new RuleHistoryRequestDTO
                {
                    MapRuleId = ruleHistory.MapRuleId,
                    IsPass = ruleHistory.IsPass,
                };

                //mockMapRuleId++;

                submitHistoryDTO.RuleHistories.Add(ruleHistoryDTO);
            }

            foreach (CommandNode commandNode in submitHistory.CommandNodes)
            {
                var commandNodeDTO = new CommandNodeDTO
                {
                    NodeIndex = commandNode.Index,
                    Type = commandNode.Type,
                    InGamePosition = new Vector2FloatDTO
                    {
                        X = commandNode.X,
                        Y = commandNode.Y,
                    },
                };

                submitHistoryDTO.CommandNodes.Add(commandNodeDTO);
            }

            foreach (CommandEdge commandEdge in submitHistory.CommandEdges)
            {
                var commandEdgeDTO = new CommandEdgeDTO
                {
                    SourceNodeIndex = commandEdge.SourceCommandIndex,
                    DestinationNodeIndex = commandEdge.DestinationCommandIndex,
                    Type = commandEdge.Type,
                };

                submitHistoryDTO.CommandEdges.Add(commandEdgeDTO);
            }

            return submitHistoryDTO;
        }
    }

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


