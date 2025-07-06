using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudCode.Apis;
using Unity.Services.CloudCode.Core;
using Unity.Services.CloudCode.Shared;
using Unity.Services.RemoteConfig.Model;

namespace CloudCode_SlotMachine;

public class SlotMachineController
{
    private const string k_slotReelId = "slot_machine_symbols";
    private const string k_playSlotMachineKey = "play_slot_machine";

    [CloudCodeFunction("SayHello")]
    public string Hello(string name)
    {
        return $"Hello, {name}!";
    }

    [CloudCodeFunction("Spin")]
    public async Task<SpinResult> Spin(IExecutionContext context, IGameApiClient gameApiClient)
    {
        SpinResult result = new();
        try
        {
            //await gameApiClient.CloudSaveData.SetItemAsync(context, context.AccessToken, context.ProjectId, context.PlayerId);

            //CurrencyModifyBalanceRequest balanceIncreaseRequest = new();
            //balanceIncreaseRequest.Amount = 300;

            //await gameApiClient.EconomyCurrencies.IncrementPlayerCurrencyBalanceAsync(context, context.AccessToken,
            //    context.ProjectId, context.PlayerId, k_creditKey, balanceIncreaseRequest);

            //PlayerPurchaseVirtualRequest request = new PlayerPurchaseVirtualRequest();
            //request.Id = k_playSlotMachineKey;
            //await gameApiClient.EconomyPurchases.MakeVirtualPurchaseAsync(context, context.AccessToken,
            //context.ProjectId, context.PlayerId, request);

            ApiResponse<SettingsDeliveryResponse> response = await gameApiClient.RemoteConfigSettings.AssignSettingsGetAsync(
                context, context.AccessToken, context.ProjectId, context.EnvironmentId, null, new List<string>() { k_slotReelId });
            
            var settings = response.Data.Configs.Settings;
            SlotReel slotReel = JsonConvert.DeserializeObject<SlotReel>(settings[k_slotReelId].ToString());

            List<string> spinResult = new();
            Random random = new();

            for (int i = 0; i < slotReel.Symbols.Length; i++)
                spinResult.Add(slotReel.Symbols[random.Next(0, slotReel.Symbols.Length)]);
            result.Spins = spinResult.ToArray();
            return result;
        }
        catch (ApiException e)
        {
            throw new Exception($"Error: {e.Message}");
        }
    }
}

public struct SlotReel
{
    public string[] Symbols;
}

public struct SpinResult
{
    public string[] Spins;
}