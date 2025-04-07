namespace DotNetInterview.Application.Items.Interfaces;

using DotNetInterview.Application.Items.QueryResults;
using DotNetInterview.Application.Items.ReadModels;

internal interface IItemProcessManager
{
    Item Process(ItemReadModel model);
}
