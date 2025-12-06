
using System.IO;
using System.Text;
using Bible2PPT.Bibles;
using Bible2PPT.Services.BibleIndexService;
using Bible2PPT.Services.BibleService.Sources;

namespace Bible2PPT.Sources;

public class LocalBible : BibleSource
{
    private const string BASE_URL = "resource/bible";
    private static readonly Encoding encoding = Encoding.GetEncoding("EUC-KR");

    public LocalBible()
    {
        Name = "로컬 성경";
    }

    public override Task<List<Bible>> GetBiblesOnlineAsync() =>
        Task.FromResult(new List<Bible>
        {
            new Bible
            {
                OnlineId = "local",
                Name = "로컬 성경",
                LanguageCode = "ko",
            }
        });
    public override async Task<List<Book>> GetBooksOnlineAsync(Bible bible)
    {
        string[] bookNames = FileManager.LoadFileNames();
        return await Task.Run(() => string2book(bookNames));
    }
    public override async Task<List<Chapter>> GetChaptersOnlineAsync(Book book)
    {
        return await Task.Run(() =>
        {
            var chapters = new List<Chapter>();
            var bookName = FileManager.remove_extension(book.OnlineId);
            var indexes = FileManager.getIndexs(bookName);
            for (var i = 1; i <= indexes.Count; i++)
            {
                chapters.Add(
                    new Chapter
                    {
                        OnlineId = $"{i}",
                        Number = i
                    }
                );
            }
            return chapters;
        });
    }
    public override async Task<List<Verse>> GetVersesOnlineAsync(Chapter chapter)
    {
        return await Task.Run(() =>
        {
            var book = chapter.Book;
            var bookName = FileManager.remove_extension(book.OnlineId);
            var indexes = FileManager.getIndexs(bookName);
            var verses = new List<Verse>();

            var startLine = indexes[chapter.Number - 1];
            var endLine = (chapter.Number < indexes.Count) ? indexes[chapter.Number] - 1 : -1;
            
            var bookPath = Path.Combine("bible", "text", book.OnlineId);
            var texts = FileManager.ReadFromToLine(bookPath, startLine, endLine);

            for (var i = 0; i < texts.Count; i++)
            {
                verses.Add(
                    new Verse
                    {
                        Number = i + 1,
                        Text = texts[i]
                    }
                );
            }
            return verses;
        });
    }

    private List<Book> string2book(string[] bookNames)
    {
        var books = new List<Book>();
        foreach (var bookName in bookNames)
        {
            var book = new Book
            {
                OnlineId = bookName,
                Name = FileManager.remove_number(FileManager.remove_extension(bookName)),
                ChapterCount = FileManager.getIndexs(FileManager.remove_extension(bookName)).Count,
            };
            book.Key = GetBookKey(book);
            books.Add(book);
        }
        return books;
    }

    private static BookKey GetBookKey(Book book) => book.Name switch
    {
        "창세기" => BookKey.Genesis,
        "출애굽기" => BookKey.Exodus,
        "레위기" => BookKey.Leviticus,
        "민수기" => BookKey.Numbers,
        "신명기" => BookKey.Deuteronomy,
        "여호수아" => BookKey.Joshua,
        "사사기" => BookKey.Judges,
        "룻기" => BookKey.Ruth,
        "사무엘상" => BookKey.ISamuel,
        "사무엘하" => BookKey.IISamuel,
        "열왕기상" => BookKey.IKings,
        "열왕기하" => BookKey.IIKings,
        "역대상" => BookKey.IChronicles,
        "역대하" => BookKey.IIChronicles,
        "에스라" => BookKey.Ezra,
        "느헤미야" => BookKey.Nehemiah,
        "에스더" => BookKey.Esther,
        "욥기" => BookKey.Job,
        "시편" => BookKey.Psalms,
        "잠언" => BookKey.Proverbs,
        "전도서" => BookKey.Ecclesiastes,
        "아가" => BookKey.SongOfSolomon,
        "이사야" => BookKey.Isaiah,
        "예레미야" => BookKey.Jeremiah,
        "예레미야애가" => BookKey.Lamentations,
        "에스겔" => BookKey.Ezekiel,
        "다니엘" => BookKey.Daniel,
        "호세아" => BookKey.Hosea,
        "요엘" => BookKey.Joel,
        "아모스" => BookKey.Amos,
        "오바댜" => BookKey.Obadiah,
        "요나" => BookKey.Jonah,
        "미가" => BookKey.Micah,
        "나훔" => BookKey.Nahum,
        "하박국" => BookKey.Habakkuk,
        "스바냐" => BookKey.Zephaniah,
        "학개" => BookKey.Haggai,
        "스가랴" => BookKey.Zechariah,
        "말라기" => BookKey.Malachi,
        "마태복음" => BookKey.Matthew,
        "마가복음" => BookKey.Mark,
        "누가복음" => BookKey.Luke,
        "요한복음" => BookKey.John,
        "사도행전" => BookKey.Acts,
        "로마서" => BookKey.Romans,
        "고린도전서" => BookKey.ICorinthians,
        "고린도후서" => BookKey.IICorinthians,
        "갈라디아서" => BookKey.Galatians,
        "에베소서" => BookKey.Ephesians,
        "빌립보서" => BookKey.Philippians,
        "골로새서" => BookKey.Colossians,
        "데살로니가전서" => BookKey.IThessalonians,
        "데살로니가후서" => BookKey.IIThessalonians,
        "디모데전서" => BookKey.ITimothy,
        "디모데후서" => BookKey.IITimothy,
        "디도서" => BookKey.Titus,
        "빌레몬서" => BookKey.Philemon,
        "히브리서" => BookKey.Hebrews,
        "야고보서" => BookKey.James,
        "베드로전서" => BookKey.IPeter,
        "베드로후서" => BookKey.IIPeter,
        "요한일서" => BookKey.IJohn,
        "요한이서" => BookKey.IIJohn,
        "요한삼서" => BookKey.IIIJohn,
        "유다서" => BookKey.Jude,
        "요한계시록" => BookKey.Revelation,
        _ => BookKey.Unknown,
    };
}
