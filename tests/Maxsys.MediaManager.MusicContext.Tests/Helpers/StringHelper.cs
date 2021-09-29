﻿namespace Maxsys.MediaManager.Tests
{
    internal static class StringHelper
    {
        private const string WORD_5050_CHARS = "Lorem ipsum dolor sit amet consectetur adipiscing elit Nam sem metus facilisis sit amet ultrices sollicitudin condimentum sed felis Interdum et malesuada fames ac ante ipsum primis in faucibus Interdum et malesuada fames ac ante ipsum primis in faucibus Sed eu erat eu velit finibus auctor aliquet vel leo In commodo tempor eros ut dignissim Nunc vel blandit est Class aptent taciti sociosqu ad litora torquent per conubia nostra per inceptos himenaeos Phasellus eu convallis mauris Sed aliquam pretium metus id posuere nibh tincidunt non Aenean bibendum leo id leo feugiat laoreetNulla sit amet viverra lorem Quisque eget nisl sagittis tempus magna vel placerat ex Aliquam in purus sed est aliquam euismod Phasellus finibus leo quis felis viverra a aliquet ex aliquam Phasellus finibus sollicitudin purus vel molestie orci iaculis eget Praesent libero risus luctus et velit vel imperdiet efficitur nisi Morbi sed lacinia odio Sed eleifend fermentum facilisis Aenean vel erat quis nisl aliquet mollis Quisque vestibulum fermentum elit Mauris porttitor neque augue in rhoncus nisi dignissim vel Class aptent taciti sociosqu ad litora torquent per conubia nostra per inceptos himenaeos Curabitur sed erat turpisSed fermentum feugiat sem sit amet semper Pellentesque blandit sapien id tempor feugiat Phasellus tincidunt hendrerit ligula a pharetra Proin faucibus lacus et augue ullamcorper nec hendrerit dui semper Sed a maximus turpis Aenean posuere nisl nec nulla consectetur ut scelerisque felis tristique Suspendisse felis ipsum tincidunt ac ultricies bibendum condimentum eget diam Integer non iaculis nulla at commodo nunc Donec vel metus in dolor vulputate porttitor non vitae neque Sed facilisis convallis libero eget pharetra enim fermentum eu Morbi sagittis rutrum ullamcorper Nam sed nisl sed dui rhoncus pulvinar Nam sed lorem a dui sodales faucibus Etiam enim dolor dignissim quis dapibus ac porta vel orciProin varius egestas odio eleifend aliquam massa placerat eu Quisque scelerisque risus sit amet tristique tristique elit elit mattis nisl eget interdum tellus mi luctus est Vivamus placerat felis maximus laoreet convallis Proin convallis eros non dignissim porta Aenean ornare augue enim id pulvinar ipsum consequat in Phasellus malesuada ex a ex tincidunt non tempor turpis tincidunt Morbi non augue eget ante consectetur bibendum Morbi pulvinar euismod nibh nec hendrerit dolor pulvinar at Aliquam id est a mauris dapibus finibus vitae a lorem Curabitur at venenatis lorem non suscipit ligula Phasellus at orci sit amet neque iaculis iaculis in non mauris Mauris sed mi ut lorem accumsan auctor vitae at magnaNulla facilisi Nulla vehicula molestie orci in semper Vestibulum eu cursus justo Praesent bibendum eget felis non vulputate Maecenas eget nulla vulputate facilisis est eget aliquet justo Donec tristique volutpat justo a ultricies Nam in lacus eget dolor mollis fermentum Nulla malesuada imperdiet libero nec rutrumMaecenas lectus tellus ultricies sed tempus at imperdiet vitae velit Praesent imperdiet arcu a quam egestas eget tincidunt metus accumsan Proin commodo fringilla ex Nam a massa pharetra venenatis felis et porta enim Mauris aliquam ante id pharetra lobortis eros nisl pretium sapien quis ullamcorper mauris quam sed nunc Nam non tellus nisi Nullam sollicitudin ut est ac sollicitudin Sed ut ex in massa malesuada vestibulum Integer at ex elit Aenean eget velit quam Orci varius natoque penatibus et magnis dis parturient montes nascetur ridiculus mus Sed dapibus est ante a aliquam tellus commodo necUt sit amet orci ut felis finibus pellentesque Donec molestie vel eros vel sollicitudin Nullam placerat id libero et mollis Nulla tempor ante et fermentum hendrerit Mauris id purus tortor Pellentesque molestie lacus at libero dictum viverra Donec et nisl ullamcorper tincidunt augue at dictum purus Nulla convallis enim eu eros euismod volutpat Fusce tristique eros a commodo porta magna nisl lobortis libero placerat accumsan mauris neque id nibh Etiam magna nisl imperdiet tincidunt suscipit vitae fringilla nec quam Curabitur quam enim laoreet nec posuere sit amet convallis ut augue Lorem ipsum dolor sit amet consectetur adipiscing elit Aenean consectetur nisi at nisi posuere ut eleifend tellus finibusPellentesque vitae neque rutrum laoreet purus et placerat ex Morbi viverra nisl sit amet sodales cursus Fusce congue lectus risus a rhoncus diam interdum nec Maecenas turpis turpis scelerisque eget molestie vitae blandit quis arcu Maecenas lacinia interdum leo id egestas eros ultrices at Nam augue purus faucibus quis rutrum id mattis sit amet est Morbi rhoncus sagittis fringilla Donec eget aliquam mauris vitae pharetra nisi Phasellus fermentum felis ut faucibus vehicula diam orci sagittis lacus sed elementum nunc nulla in nuncEtiam sagittis tellus nisl sit amet gravida urna commodo sed Lorem ipsum dolor sit amet consectetur adipiscing elit Donec sagittis lectus luctus arcu accumsan semper Vestibulum pellentesque tristique efficitur Cras auctor ex sed vulputate faucibus nibh lectus vestibulum";

        /// <summary>
        /// Returns a string with a specific <paramref name="length"/>
        /// </summary>
        /// <param name="length">is the length of the required string. Must be a number between 0 and 5050</param>
        /// <returns>a string with a required length</returns>
        internal static string GetWord(int length)
        {
            if (length >= WORD_5050_CHARS.Length)
                length = WORD_5050_CHARS.Length;
            
            if (length < 0)
                length = 0;

            return WORD_5050_CHARS.Substring(0, length);
        }
    }
}
