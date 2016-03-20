﻿namespace Rocket.Chat.Net.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;

    using Rocket.Chat.Net.Driver;
    using Rocket.Chat.Net.Models;
    using Rocket.Chat.Net.Models.Results;

    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public interface IRocketChatDriver : IDisposable
    {
        event MessageReceived MessageReceived;

        /// <summary>
        /// Connect via the DDP protocol using WebSockets
        /// </summary>
        /// <returns></returns>
        Task ConnectAsync();

        /// <summary>
        /// Subscribe to messages from given room
        /// </summary>
        /// <param name="roomId">The room to listen to. Null will listen to all authorized rooms. This may or may not be correct. </param>
        /// <returns></returns>
        Task SubscribeToRoomAsync(string roomId = null);

        /// <summary>
        /// Login with email
        /// </summary>
        /// <param name="email">Email to use</param>
        /// <param name="password">Plaintext password to use (will be SHA-256 before sending)</param>
        /// <returns></returns>
        Task<LoginResult> LoginWithEmailAsync(string email, string password);

        /// <summary>
        /// Login with LDAP
        /// </summary>
        /// <param name="username">Email/Username to use</param>
        /// <param name="password">Plaintext password to use</param>
        /// <returns></returns>
        Task<LoginResult> LoginWithLdapAsync(string username, string password);

        /// <summary>
        /// Login with username
        /// </summary>
        /// <param name="username">Username to use</param>
        /// <param name="password">Plaintext password to use (will be SHA-256 before sending)</param>
        /// <returns></returns>
        Task<LoginResult> LoginWithUsernameAsync(string username, string password);

        /// <summary>
        /// Resume a login session
        /// </summary>
        /// <param name="sessionToken">Active token given from a previous login</param>
        /// <returns></returns>
        Task<LoginResult> LoginResumeAsync(string sessionToken);

        /// <summary>
        /// Login with a ILogin object
        /// </summary>
        /// <param name="loginOption">Login option to use</param>
        /// <returns></returns>
        Task<LoginResult> LoginAsync(ILoginOption loginOption);

        /// <summary>
        /// Get roomId by either roomId or room name
        /// </summary>
        /// <param name="roomIdOrName">Room name or roomId</param>
        /// <returns></returns>
        Task<string> GetRoomIdAsync(string roomIdOrName);

        /// <summary>
        /// Joins a room, no effect if already joined
        /// </summary>
        /// <param name="roomId">The room to join</param>
        /// <returns></returns>
        Task<dynamic> JoinRoomAsync(string roomId);

        /// <summary>
        /// Send a message to a room
        /// </summary>
        /// <param name="text">Text of the message</param>
        /// <param name="roomId">The room to send to</param>
        /// <returns></returns>
        Task<dynamic> SendMessageAsync(string text, string roomId);

        /// <summary>
        /// Edit a message by messageId
        /// </summary>
        /// <param name="messageId">Message to update</param>
        /// <param name="roomId">Room that the message exists in</param>
        /// <param name="newMessage">Update the message to this</param>
        /// <returns></returns>
        Task<object> UpdateMessageAsync(string messageId, string roomId, string newMessage);

        /// <summary>
        /// Load the message history of a room ordered by timestamp newest first
        /// </summary>
        /// <param name="roomId">Room to list history of</param>
        /// <param name="end">No idea, something with timespan</param>
        /// <param name="limit">Max number of messages to load</param>
        /// <param name="ls">No idea, something with timespan, maybe less than?</param>
        /// <returns></returns>
        Task<List<RocketMessage>> LoadMessagesAsync(string roomId, DateTime? end = null, int? limit = 20,
                                                    string ls = null);

        /// <summary>
        /// Delete message
        /// </summary>
        /// <param name="messageId">The message to delete</param>
        /// <param name="roomId">The room where that message sits</param>
        /// <returns>The private RoomId</returns>
        Task<string> DeleteMessageAsync(string messageId, string roomId);

        /// <summary>
        /// Create a private message room with target user
        /// </summary>
        /// <param name="username">The user to create a private message room for</param>
        /// <returns>The private RoomId</returns>
        Task<string> CreatePrivateMessageAsync(string username);

        /// <summary>
        /// List authorized channels
        /// </summary>
        /// <returns></returns>
        Task<object> ChannelListAsync();

        /// <summary>
        /// Called when the Ddp Clients reconnects, can be used to log back in or resubscribe.
        /// </summary>
        event DdpReconnect DdpReconnect;

        /// <summary>
        /// Subscribe to the filtered users list. Can be called multiple times with different filter information.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        Task SubscribeToFilteredUsersAsync(string username = "");

        /// <summary>
        /// Ping Rocket.Chat server (a kind of keep alive). 
        /// Can be used at any time when connected. 
        /// </summary>
        /// <returns></returns>
        Task PingAsync();

        /// <summary>
        /// Searches the messages for the given room.
        /// </summary>
        /// <param name="query">Query to search for (can use 'from:', 'mention:', etc.)</param>
        /// <param name="roomId">RoomId to search for</param>
        /// <param name="limit">Limit the number of messages (default 100)</param>
        /// <returns></returns>
        Task<List<RocketMessage>> SearchMessagesAsync(string query, string roomId, int limit = 100);

        /// <summary>
        /// Get a streaming collection.
        /// </summary>
        /// <param name="collectionName">Name of the collection to get (e.g. users).</param>
        /// <returns>Collection requested, null if it does not exist.</returns>
        StreamCollection GetCollection(string collectionName);
    }
}