// synthesis patch
SinOsc osc => JCRev r => ADSR env => dac;
TriOsc osc2 => ADSR env2 => dac;
TriOsc osc3 => dac;
TriOsc osc4 => dac;
TriOsc osc5 => dac;
TriOsc osc6 => dac;
// run white noise through envelope
Noise n => Envelope e => dac;

1::second => dur playTime;
1::second => dur playTimeClimax;
38::second => dur playIntro;

// for ringing sounds
0.5 => osc.gain;
0.5 => osc2.gain;

[0,4,7,12] @=> int major[];
[0,3,7,12,15,17] @=> int minor[];
[12,7,3,0] @=> int minor2[];

[300,280,215,194] @=> int seq[];
[300,280,300,194] @=> int seq2[];

60 => int offset; //C4
85 => int offset2;
65 => int offset3;
55 => int offset4;
60 => int offset5;

.0 => n.gain;
.5 => osc2.gain;
.0 => osc3.gain;
.0 => osc4.gain;
.0 => osc5.gain;
.0 => osc6.gain;


// ---------------- start ------------------


// little bells - dreamy
for(0 => int i; i < 2; i++)
{
    for(0 => int j; j < 4; j++) {
        Std.mtof(minor[j] + offset2) => osc.freq;
        1 => env.keyOn;
        playTime => now;
    }
}
1 => env.keyOff;

// xylophone
for(0 => int i; i < 2; i++)
{   
    //1 => env2.keyOff;
    for(0 => int j; j < 4; j++) {
        Std.mtof(major[j] + offset) => osc.freq;
        1 => env.keyOn;
        playTime => now;
    }
}

// eerie alien
for (0 => int i; i < 2; i++)
{
    for (0 => int j; j < 4; j++)
    {
        seq[j] => osc.freq;
        playTime => now;
    }
    
    for (0 => int j; j < 2; j++)
    {
        seq[j] => osc.freq;
        playTime => now;
    }
    seq[2] => osc.freq;
    playTime * 2 => now;
}
1 => env.keyOff;

// synthesizer
//for(0 => int i; i < 2; i++)
//{
    //for(0 => int j; j < 4; j++)
    //{
        //Std.mtof(minor[j] + offset) => osc2.freq;
        //1 => env2.keyOn;
        //playTime => now;
    //}
//}
//1 => env2.keyOff;

//------------------ climax --------------------

while( playTimeClimax > playTimeClimax / 12 )
{
    // randomize gain
    Math.random2f(.05,.1) => n.gain;
    // start attack
    e.keyOn(); 
    
    // synthesizer
    for(1 => int i; i < 4; i++)
    {
        // shorten interval between soundss
        playTimeClimax / i  => playTimeClimax;
        
        for(0 => int j; j < 4; j++)
        {
            Std.mtof(minor[j] + offset) => osc2.freq;
            1 => env2.keyOn;
            playTimeClimax => now;
        }
        
        for(0 => int j; j < 4; j++)
        {
            Std.mtof(minor2[j] + offset) => osc2.freq;
            1 => env2.keyOn;
            playTimeClimax => now;
        } 
    }
    1 => env2.keyOff;
}
e.keyOff();
.0 => n.gain;

.5 => osc2.gain;
.5 => osc3.gain;
.5 => osc4.gain;
.5 => osc5.gain;
.5 => osc6.gain;

Std.mtof(minor[0] + offset) => osc2.freq;
Std.mtof(minor[1] + offset) => osc3.freq;
Std.mtof(minor[2] + offset) => osc4.freq;
Std.mtof(minor[3] + offset) => osc5.freq;
Std.mtof(minor[4] + offset) => osc6.freq;

4::second => now;

Std.mtof(minor[0] + offset3) => osc2.freq;
Std.mtof(minor[1] + offset3) => osc3.freq;
Std.mtof(minor[2] + offset3) => osc4.freq;
Std.mtof(minor[3] + offset3) => osc5.freq;
Std.mtof(minor[4] + offset3) => osc6.freq;
2::second => now;

Std.mtof(minor[0] + offset4) => osc2.freq;
Std.mtof(minor[1] + offset4) => osc3.freq;
Std.mtof(minor[2] + offset4) => osc4.freq;
Std.mtof(minor[3] + offset4) => osc5.freq;
Std.mtof(minor[4] + offset4) => osc6.freq;
4::second => now;

Std.mtof(minor[0] + offset5) => osc2.freq;
Std.mtof(minor[1] + offset5) => osc3.freq;
Std.mtof(minor[2] + offset5) => osc4.freq;
Std.mtof(minor[3] + offset5) => osc5.freq;
Std.mtof(minor[4] + offset5) => osc6.freq;
6::second => now;

.0 => osc2.gain;
.0 => osc3.gain;
.0 => osc4.gain;
.0 => osc5.gain;
.0 => osc6.gain;

1::second => now;

//------------------- outro ----------------------

// eerie alien
for (0 => int i; i < 2; i++)
{
    for (0 => int j; j < 4; j++)
    {
        seq[j] => osc.freq;
        1 => env.keyOn;
        playTime => now;
    }
    
    for (0 => int j; j < 2; j++)
    {
        seq[j] => osc.freq;
        1 => env.keyOn;
        playTime => now;
    }
    seq[2] => osc.freq;
    playTime * 2 => now;
}
1 => env.keyOff;

// xylophone
for(0 => int i; i < 2; i++)
{   
    //1 => env2.keyOff;
    for(0 => int j; j < 4; j++) {
        Std.mtof(major[j] + offset) => osc.freq;
        1 => env.keyOn;
        playTime => now;
    }
}

// little bells - dreamy
for(0 => int i; i < 2; i++)
{
    for(0 => int j; j < 4; j++) {
        playTime * 1.1 => playTime;
        Std.mtof(minor[j] + offset2) => osc.freq;
        1 => env.keyOn;
        playTime => now;
    }
}
1 => env.keyOff;


