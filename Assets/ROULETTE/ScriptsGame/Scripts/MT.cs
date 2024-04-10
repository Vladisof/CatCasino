using System;

namespace Scripts.ROULETTE.ScriptsGame.Scripts
{
  public class MT
  {
    private const ulong N = 624;
    private const ulong M = 397;
    private const ulong MATRIX_A = 0x9908B0DFUL;
    private const ulong UPPER_MASK = 0x80000000UL;
    private const ulong LOWER_MASK = 0X7FFFFFFFUL;
    private const uint DEFAULT_SEED = 4357;

    private static ulong [] mt = new ulong[N + 1];
    private static ulong mti = N + 1;

    public MT()
    {
      ulong [] init = new ulong[4];
      init[0] = 0x123;
      init[1] = 0x234;
      init[2] = 0x345;
      init[3] = 0x456;
      ulong length = 4;
      init_by_array(init, length);
    }

    private static void init_genrand (ulong s)
    {
      mt[0] = s & 0xffffffffUL;

      for (mti = 1; mti < N; mti++)
      {
        mt[mti] = (1812433253UL * (mt[mti - 1] ^ (mt[mti - 1] >> 30)) + mti);
        mt[mti] &= 0xffffffffUL;
      }
    }

    private void init_by_array (ulong [] init_key, ulong key_length)
    {
      ulong i,
        j,
        k;

      init_genrand(19650218UL);
      i = 1;
      j = 0;
      k = (N > key_length ? N : key_length);

      for (; k > 0; k--)
      {
        mt[i] = (mt[i] ^ ((mt[i - 1] ^ (mt[i - 1] >> 30)) * 1664525UL)) + init_key[j] + j;
        mt[i] &= 0xffffffffUL;
        i++;
        j++;

        if (i >= N)
        {
          mt[0] = mt[N - 1];
          i = 1;
        }

        if (j >= key_length)
          j = 0;
      }

      for (k = N - 1; k > 0; k--)
      {
        mt[i] = (mt[i] ^ ((mt[i - 1] ^ (mt[i - 1] >> 30)) * 1566083941UL)) - i;
        mt[i] &= 0xffffffffUL;
        i++;

        if (i >= N)
        {
          mt[0] = mt[N - 1];
          i = 1;
        }
      }

      mt[0] = 0x80000000UL;
    }

    public void lllll()
    {}

    public long genrand_int31()
    {
      return (long)(genrand_int32() >> 1);
    }

    public double genrand_real1()
    {
      return (double)genrand_int32() * (1.0 / 4294967295.0);
    }

    public double genrand_real2()
    {
      return (double)genrand_int32() * (1.0 / 4294967296.0);
    }

    public double genrand_real3()
    {
      return (((double)genrand_int32()) + 0.5) * (1.0 / 4294967296.0);
    }

    public double genrand_res53()
    {
      ulong a = genrand_int32() >> 5;
      ulong b = genrand_int32() >> 6;
      return (double)(a * 67108864.0 + b) * (1.0 / 9007199254740992.0);
    }

    public ulong genrand_int32()
    {
      ulong y = 0;
      ulong [] mag01 = new ulong[2];
      mag01[0] = 0x0UL;
      mag01[1] = MATRIX_A;

      if (mti >= N)
      {
        ulong kk;

        if (mti == N + 1)
          init_genrand(5489UL);

        for (kk = 0; kk < N - M; kk++)
        {
          y = (mt[kk] & UPPER_MASK) | (mt[kk + 1] & LOWER_MASK);
          mt[kk] = mt[kk + M] ^ (y >> 1) ^ mag01[y & 0x1UL];
        }

        for (; kk < N - 1; kk++)
        {
          y = (mt[kk] & UPPER_MASK) | (mt[kk + 1] & LOWER_MASK);
          mt[kk] = mt[kk - 227] ^ (y >> 1) ^ mag01[y & 0x1UL];
        }

        y = (mt[N - 1] & UPPER_MASK) | (mt[0] & LOWER_MASK);
        mt[N - 1] = mt[M - 1] ^ (y >> 1) ^ mag01[y & 0x1UL];

        mti = 0;
      }

      y = mt[mti++];
      y ^= (y >> 11);
      y ^= (y << 7) & 0x9d2c5680UL;
      y ^= (y << 15) & 0xefc60000UL;
      y ^= (y >> 18);

      return y;
    }

    public int RandomRange (int lo, int hi)
    {
      return (Math.Abs((int)genrand_int32() % (hi - lo + 1)) + lo);
    }

  }
}