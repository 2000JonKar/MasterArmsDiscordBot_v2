using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MasterArmsDiscordBot_v2
{
	public class CommandsList : BaseCommandModule
	{
		// Roles that cannot be self-assigned
		string[] blacklistedRoles =
		{
			"Admin",
			"Moderator",
			"IP:s",
			"MissionBuilders",
			"Supporters",
			"CoreMembers",
			"AirshowPilots",
			"FlightLeads",
			"Members",
			"AWACS",
			"ATC"
		};

		[Command("AddRole")] // Command to be executed after the prefix
		[RequireRoles(RoleCheckMode.All, "Admin")] // Require executor to have the Admin role
		public async Task AddRole(CommandContext ctx, string roleName)
		{
			var commandMessage = ctx.Message; // Store the command message
			var guild = ctx.Guild; // Store the guild the command message was sent in
			var role = await guild.CreateRoleAsync(roleName).ConfigureAwait(false); // Create role in guild using name specified in command
			var confirmationMessage = await ctx.Channel.SendMessageAsync($"Created {role.Name}.").ConfigureAwait(false); // Send a confirmation message that the role has been created
			await Task.Delay(4000).ConfigureAwait(false); // Wait 4 seconds
			await commandMessage.DeleteAsync().ConfigureAwait(false); // Delete the command message
			await confirmationMessage.DeleteAsync().ConfigureAwait(false); // Delete the confirmation message
		}

		[Command("Role")] // Command to be executed after the prefix
		[Description("Join/Leave specified role")] // Description of the command
		public async Task Role(CommandContext ctx, DiscordRole RoleName) 
		{
			var commandMessage = ctx.Message; // Store the command message
			var caller = ctx.Member; // Store the executor
			var roles = ctx.Guild.Roles.Values.ToList(); // Make list with all roles in the server
			var roleCheck = roles.Contains(RoleName); // Check if desired role exists in list
			var getRoleName = RoleName.Name; // Store the name of the desired role
			
			if (roleCheck == true && !(blacklistedRoles.Contains(getRoleName))) // If role exists and can be self-assigned
			{
				if (caller.Roles.Contains(RoleName) == false) // If executor does not have role already
				{
					await caller.GrantRoleAsync(RoleName).ConfigureAwait(false); // Grant role to executor
					var grantConfirmation = await ctx.Channel.SendMessageAsync($"Assigned {RoleName.Name} to {ctx.Member.DisplayName}.").ConfigureAwait(false); // Send a confirmation message
					await Task.Delay(4000).ConfigureAwait(false); // Wait 4 seconds
					await grantConfirmation.DeleteAsync().ConfigureAwait(false); // Delete confirmation
				}
				else if (caller.Roles.Contains(RoleName) == true) // If executor has role already
				{
					await caller.RevokeRoleAsync(RoleName).ConfigureAwait(false); // Revoke role from executor
					var revokeConfirmation = await ctx.Channel.SendMessageAsync($"Revoked {RoleName.Name} from {ctx.Member.DisplayName}.").ConfigureAwait(false); // Send a confirmation message
					await Task.Delay(4000).ConfigureAwait(false); // Wait 4 seconds
					await revokeConfirmation.DeleteAsync().ConfigureAwait(false); // Delete confirmation
				}
			}
			else if (roleCheck == true && blacklistedRoles.Contains(getRoleName)) // If role exists but cannot be self-assigned
			{
				var notAllowed = await ctx.Channel.SendMessageAsync("That role cannot be assigned by command.").ConfigureAwait(false); // Send message
				await Task.Delay(4000).ConfigureAwait(false); // Wait 4 seconds
				await notAllowed.DeleteAsync().ConfigureAwait(false); // Delete message
			}

			await commandMessage.DeleteAsync().ConfigureAwait(false); // Delete the command message
		}
	}
}
