using Flurl;
using pinboard.net.Models;
using pinboard.net.Util;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace pinboard.net.Endpoints
{
    public class Notes : Endpoint
    {
        public Notes(string apiToken, HttpClient httpClient) 
            : base(apiToken, httpClient)
        { }

        /// <summary>
        /// Returns a list of the user's notes
        /// </summary>
        /// <returns>All the notes</returns>
        public Task<AllNotes> List()
        {
            var url = NotesURL.AppendPathSegment("list");
            return MakeRequestAsync<AllNotes>(url);
        }

        /// <summary>
        /// Returns an individual user note. The ID property is a 20 character long sha1 hash of the note text.
        /// </summary>
        /// <param name="id">Note ID</param>
        /// <returns>Specific note</returns>
        public Task<Note> Note(string id)
        {
            if (id.IsEmpty())
                throw new ArgumentException("ID can't be empty");

            var url = NotesURL
                        .AppendPathSegment(id);

            return MakeRequestAsync<Note>(url);
        }
    }
}
