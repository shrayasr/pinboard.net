# Pinboard.net

A fully featured C# wrapper for the [pinboard.in](https://pinboard.in) API.

## Installation

    PM> Install-Package pinboard.net

## Usage

To start, retrieve the Pinboard API Token from the
[password](https://pinboard.in/settings/password) page on the website.

The class that starts it all is `PinboardAPI`. It implements `IDisposable` and
is best used within an `using` block like so:

```CSharp
using (var pb = new PinboardAPI(apiToken))
{
  // ...
}
```

This internally creates and reuses **one** instance of `HttpClient` per
instance of `PinboardAPI`.

The `pb` object can now be used to make calls to the Pinboard API

### Posts

#### Update

Returns the most recent time a bookmark was added, updated or deleted.
Use this before calling All to see if the data has changed since the last
fetch.

```CSharp
pb.Posts.GetLastUpdate()
```

#### Add a bookmark

```CSharp
var bookmark = new Bookmark
{
  Url = "http://linkur.co.in",
  Description = "Bookmarking for groups!",
  Extended = "",
  Tags = new List<string> { "bookmarking", "web", "tools" },
  dt = DateTime.Now,
  Shared = true,
  ToRead = false
};

pb.Posts.Add(bookmark);
```

#### Update a bookmark

```CSharp
// Get the bookmark first
var bookmark = pb.Posts.All().FirstOrDefault();

bookmark.Extended = "Nothing does group bookmarking better";
bookmark.Tags.Add("free");

pb.Posts.Update(bookmark)
```

#### Delete a bookmark

```CSharp
pb.Posts.Delete("http://linkur.co.in");
```

#### Get posts matching parameters

Returns one or more posts on a single day matching the arguments. If no
date or url is given, date of most recent bookmark will be used.

It can be filtered by:

- Tags
- Date
- URL

```CSharp
pb.Posts.Get();
```

#### Get recent bookmarks

Returns a list of the user's most recent posts, filtered by tag.

```CSharp
pb.Posts.Recent(tags: new List<string> { "programming", "dotnet" });
```

#### Get all bookmarks

Returns all bookmarks in the user's account.

It can be filtered by:

- Tags
- Offset
- Number of results
- From date
- To date

```CSharp
pb.Posts.All()
```

### Tags

#### Get all tags

This also returns the number of times each tag has been used

```CSharp
pb.Tags.Get()
```

#### Get suggested tags for a URL

```CSharp
pb.Posts.Suggest("https://linkur.co.in")
```

#### Delete a tag

```CSharp
pb.Tags.Delete("prugramming");
```

#### Rename a tag

```CSharp
pb.Tags.Rename("pithon", "python");
```

### Users

#### Get secret

Returns the user's secret RSS key (for viewing private feeds)

```CSharp
pb.Users.Secret()
```

#### Get API Token

Returns the user's API token (for making API calls without a password)

```CSharp
pb.Users.ApiToken()
```

### Notes

#### Get all notes

```CSharp
pb.Notes.List()
```

#### Get details of a single note

```CSharp
pb.Notes.Note("foobar")
```
