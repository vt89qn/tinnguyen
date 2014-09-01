// Decompiled by Jad v1.5.8e. Copyright 2001 Pavel Kouznetsov.
// Jad home page: http://www.geocities.com/kpdus/jad.html
// Decompiler options: braces fieldsfirst space lnc 

package com.instagram.strings;


import android.util.Log;

public class StringBridge
{
    public static native String getInstagramString(String s);

    public static native String getSignatureString(byte abyte0[]);

    static {
        try {
            System.loadLibrary("cryptox");
            System.loadLibrary("scrambler");
            System.loadLibrary("strings");

        } catch (UnsatisfiedLinkError ule) {
            Log.e("10", "******** Could not load native library nativeim ********");
            Log.e("10", "******** Could not load native library nativeim ********", ule);
            Log.e("10", "******** Could not load native library nativeim ********");
        } catch (Throwable t) {
            Log.e("10", "******** Failed to load native dictionary library ********");
            Log.e("10", "******** Failed to load native dictionary library *******", t);
            Log.e("10", "******** Failed to load native dictionary library ********");
        }
    }
}
