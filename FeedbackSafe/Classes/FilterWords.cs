using System.Text.RegularExpressions;

namespace FeedbackSafe
{
    public class FilterWords
    {
        public static bool HasBadWords(string inputWords)
        {
            var wordFilter =
                new Regex(
                    "(ahole|anus|ash0le|ash0les|asholes|ass|Assface|assh0le|assh0lez|asshole|assholes|assholz|asswipe|azzhole|bassterds|bastard|bastards|bastardz|basterds|basterdz|Biatch|bitch|bitches|Blow Job|c0ck|c0cks|cock|cockhead|cock-head|cocks|CockSucker|cock-sucker|crap|cum|cunt|cunts|cuntz|dick|dildo|dildos|dyke|enema|f u c k|f u c k e r|fag|fag1t|faget|fagg1t|faggit|faggot|fagit|fags|fagz|faig|faigs|fart|flipping the bird|fuck|fucker|fuckin|fucking|fucks|Fudge Packer|fuk|fuker|God-damned|jackoff|jerk-off|jiss|jizm|jizz|kunt|Lesbian|Motha Fucker|Motha Fuker|Motha Fukkah|Motha Fukker|Mother Fucker|Mother Fukah|Mother Fuker|Mother Fukkah|Mother Fukker|mother-fucker|Mutha Fucker|Mutha Fukah|Mutha Fuker|Mutha Fukkah|Mutha Fukker|n1gr|nastt|nigger|nigur|niiger|niigr|Phuc|Phuck|Phuk|Phuker|Phukker|Poonani|pr1c|pr1ck|pr1k|pusse|pussee|pussy|puuke|puuker|queer|queers|queerz|qweers|qweerz|qweir|recktum|rectum|sadist|scank|schlong|screwing|semen|Sh!t|sh1t|sh1ter|sh1ts|sh1tter|sh1tz|shit|shits|shitter|Shitty|Shity|shitz|Shyt|Shyte|Shytty|Shyty|skanck|skank|skankee|skankey|skanks|Skanky|slut|sluts|Slutty|slutz|son-of-a-bitch|tit|turd|bitch|blowjob|clit|fuck|shit|ass|asshole|bastard|clits|cock|cum|cunt|dildo|fcuk|fuk|motherfucker|nazi|nigga|nigger|nutsack|phuck|pimpis|pusse|pussy|scrotum|teets|tits|boobs|b00bs|testical|testicle|jackoff|wank|whoar|whore|damn|dyke|fuck|shit|bitch|Cock|cunt|dick|fag|fuk|gay|gook|jizz|kike|lesbo|nazis|queef|spic|splooge|twat)");
            return wordFilter.IsMatch(inputWords);
        }

        public static string ChangeBadWords(string inputWords)
        {
            var wordFilter =
                new Regex(
                    "(ahole|anus|ash0le|ash0les|asholes|ass|Assface|assh0le|assh0lez|asshole|assholes|assholz|asswipe|azzhole|bassterds|bastard|bastards|bastardz|basterds|basterdz|Biatch|bitch|bitches|Blow Job|c0ck|c0cks|cock|cockhead|cock-head|cocks|CockSucker|cock-sucker|crap|cum|cunt|cunts|cuntz|dick|dildo|dildos|dyke|enema|f u c k|f u c k e r|fag|fag1t|faget|fagg1t|faggit|faggot|fagit|fags|fagz|faig|faigs|fart|flipping the bird|fuck|fucker|fuckin|fucking|fucks|Fudge Packer|fuk|fuker|God-damned|jackoff|jerk-off|jiss|jizm|jizz|kunt|Lesbian|Motha Fucker|Motha Fuker|Motha Fukkah|Motha Fukker|Mother Fucker|Mother Fukah|Mother Fuker|Mother Fukkah|Mother Fukker|mother-fucker|Mutha Fucker|Mutha Fukah|Mutha Fuker|Mutha Fukkah|Mutha Fukker|n1gr|nastt|nigger|nigur|niiger|niigr|Phuc|Phuck|Phuk|Phuker|Phukker|Poonani|pr1c|pr1ck|pr1k|pusse|pussee|pussy|puuke|puuker|queer|queers|queerz|qweers|qweerz|qweir|recktum|rectum|sadist|scank|schlong|screwing|semen|Sh!t|sh1t|sh1ter|sh1ts|sh1tter|sh1tz|shit|shits|shitter|Shitty|Shity|shitz|Shyt|Shyte|Shytty|Shyty|skanck|skank|skankee|skankey|skanks|Skanky|slut|sluts|Slutty|slutz|son-of-a-bitch|tit|turd|bitch|blowjob|clit|fuck|shit|ass|asshole|bastard|clits|cock|cum|cunt|dildo|fcuk|fuk|motherfucker|nazi|nigga|nigger|nutsack|phuck|pimpis|pusse|pussy|scrotum|teets|tits|boobs|b00bs|testical|testicle|jackoff|wank|whoar|whore|damn|dyke|fuck|shit|bitch|Cock|cunt|dick|fag|fuk|gay|gook|jizz|kike|lesbo|nazis|queef|spic|splooge|twat)");
            string lowerwords = inputWords.ToLower();
            return wordFilter.Replace(lowerwords, "****");
        }
    }
}