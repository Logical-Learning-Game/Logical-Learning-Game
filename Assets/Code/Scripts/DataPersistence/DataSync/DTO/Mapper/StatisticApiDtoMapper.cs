using System.Collections.Generic;

namespace Unity.Game.SaveSystem
{
    public class StatisticApiDtoMapper
    {
        public GameSessionHistoryRequestDto ToDto(GameSession gameSession)
        {
            var gameSessionRequestHistoryDTO = new GameSessionHistoryRequestDto
            {
                MapId = 1,
                StartDatetime = gameSession.StartDatetime.DateTime,
                EndDatetime = gameSession.EndDatetime.DateTime,
                SubmitHistories = new List<SubmitHistoryRequestDto>()
            };

            foreach (SubmitHistory submitHistory in gameSession.SubmitHistories)
            {
                var submitHistoryDTO = new SubmitHistoryRequestDto
                {
                    IsFinited = submitHistory.IsFinited,
                    IsCompleted = submitHistory.IsCompleted,
                    CommandMedal = submitHistory.CommandMedal,
                    ActionMedal = submitHistory.ActionMedal,
                    SubmitDatetime = submitHistory.SubmitDatetime.DateTime,
                    StateValue = new StateValueRequestDto
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
                    RuleHistories = new List<RuleHistoryRequestDto>(),
                    CommandNodes = new List<CommandNodeRequestDto>(),
                    CommandEdges = new List<CommandEdgeRequestDto>(),
                };

                // mock map rule id!!!
                long mockMapRuleId = 1;
                foreach (RuleHistory ruleHistory in submitHistory.RuleHistories)
                {
                    var ruleHistoryDTO = new RuleHistoryRequestDto
                    {
                        MapRuleId = mockMapRuleId,
                        IsPass = ruleHistory.IsPass,
                    };

                    mockMapRuleId++;

                    submitHistoryDTO.RuleHistories.Add(ruleHistoryDTO);
                }

                foreach (CommandNode commandNode in submitHistory.CommandNodes)
                {
                    var commandNodeDTO = new CommandNodeRequestDto
                    {
                        NodeIndex = commandNode.Index,
                        Type = commandNode.Type,
                        InGamePosition = new Vector2FloatDto
                        {
                            X = commandNode.X,
                            Y = commandNode.Y,
                        },
                    };

                    submitHistoryDTO.CommandNodes.Add(commandNodeDTO);
                }

                foreach (CommandEdge commandEdge in submitHistory.CommandEdges)
                {
                    var commandEdgeDTO = new CommandEdgeRequestDto
                    {
                        SourceNodeIndex = commandEdge.SourceCommandIndex,
                        DestinationNodeIndex = commandEdge.DestinationCommandIndex,
                        Type = commandEdge.Type,
                    };

                    submitHistoryDTO.CommandEdges.Add(commandEdgeDTO);
                }

                gameSessionRequestHistoryDTO.SubmitHistories.Add(submitHistoryDTO);
            }

            return gameSessionRequestHistoryDTO;
        }
    }
}


